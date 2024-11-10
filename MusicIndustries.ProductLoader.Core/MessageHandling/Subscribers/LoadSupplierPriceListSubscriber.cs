using System;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using MusicIndustries.ProductLoader.ExcelProcessing;
using MusicIndustries.ProductLoader.FileProviders;
using MusicIndustries.ProductLoader.MessageHandling.Infrastructure;
using MusicIndustries.ProductLoader.MessageHandling.Messages;
using MusicIndustries.ProductLoader.Repositories;
using Newtonsoft.Json;
using ProductLoader.DataContracts.SupplierPriceLists;
using ProductLoader.DataContracts.SupplierPriceLists.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MusicIndustries.ProductLoader.MessageHandling.Subscribers
{
    public class LoadSupplierPriceListSubscriber : ISubscriber<LoadSupplierPriceList>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IProcessExcelData _processExcelData;
        private readonly IPublisher<LoadSupplierPriceList> _retryPublisher;
        private readonly IPublisher<LoadProductDetails> _productDetailsPublisher;
        private readonly IFileImportProvider _fileImportProvider;
        private readonly IRepository<PriceListRow> _productLoaderRepository;
        private readonly IServiceProvider _serviceProvider;

        public LoadSupplierPriceListSubscriber(
            IProcessExcelData processExcelData,
            IPublisher<LoadSupplierPriceList> retryPublisher,
            IFileImportProvider fileImportProvider,
            IRepository<PriceListRow> productLoaderRepository,
            IServiceProvider serviceProvider,
            IPublisher<LoadProductDetails> productDetailsPublisher)
        {
            _processExcelData = processExcelData;
            _retryPublisher = retryPublisher;
            _fileImportProvider = fileImportProvider;
            _productLoaderRepository = productLoaderRepository;
            _serviceProvider = serviceProvider;
            _productDetailsPublisher = productDetailsPublisher;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare($"{nameof(LoadSupplierPriceList)}", ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, $"{nameof(LoadSupplierPriceList)}", "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received: {message}");
                var messageObject = JsonConvert.DeserializeObject<LoadSupplierPriceList>(message);
                await HandleAsync(messageObject!);
            };
            _channel.BasicConsume(queueName, true, consumer);
        }

        public async Task HandleAsync(LoadSupplierPriceList message)
        {
            // wait for the next retry
            await Task.Delay(message.nextRetryTime);
            try
            {

                switch (message.Supplier)
                {

                    case Supplier.AudioSure:
                    {
                        await SaveSupplierPriceList<AudioSurePriceList>(message);
                        break;
                    }
                    case Supplier.RockitDistribution:
                    {
                        await SaveSupplierPriceList<RockitPriceList>(message);
                        break;
                    }
                    case Supplier.MusicalDistributors:
                    {
                         await SaveSupplierPriceList<MdPriceList>(message);
                            break;
                    }
                    case Supplier.BkPercussion:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(ex.Message);
                // Retry the operation
                if (message.retryCount >= 3) throw;

                message.retryCount++;
                message.nextRetryTime = message.nextRetryTime.Add(TimeSpan.FromSeconds(10));
                await _retryPublisher.PublishAsync(message);
            }
        }
        private async Task SaveSupplierPriceList<T>(LoadSupplierPriceList message) where T : IExcelModel, new()
        {

            await using var stream = await _fileImportProvider.GetFileStreamAsync(message.Supplier);
            if (stream is null)
            {
                return;
            }
            // Load supplier price list
            var supplierPriceList = await _processExcelData.ReadExcelFileAsync<T>(stream);
            // Save supplier price list
            foreach (var supplierPriceListItem in supplierPriceList)
            {
                await _productLoaderRepository.AddItem(supplierPriceListItem.MapToPriceListRow());
            }
            await _fileImportProvider.ArchiveProcessedFile(message.Supplier);
        }
    }
}
