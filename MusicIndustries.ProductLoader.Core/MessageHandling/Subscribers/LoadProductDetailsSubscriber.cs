using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicIndustries.ProductLoader.AiOpProcessing;
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
    public class LoadProductDetailsSubscriber : ISubscriber<LoadProductDetails>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IPublisher<LoadProductDetails> _retryPublisher;
        private readonly IProcessAiWorkloads _processAiWorkloads;
        private readonly IRepository<PriceListRow> _priceListRepository;
        private readonly IServiceProvider _serviceProvider;

        public LoadProductDetailsSubscriber(
            IPublisher<LoadProductDetails> retryPublisher,
            IServiceProvider serviceProvider,
            IProcessAiWorkloads processAiWorkloads,
            IRepository<PriceListRow> priceListRepository)
        {
            _retryPublisher = retryPublisher;
            _serviceProvider = serviceProvider;
            _processAiWorkloads = processAiWorkloads;
            _priceListRepository = priceListRepository;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare($"{nameof(LoadProductDetails)}", ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, $"{nameof(LoadProductDetails)}", "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received: {message}");
                var messageObject = JsonConvert.DeserializeObject<LoadProductDetails>(message);
                await HandleAsync(messageObject!);
            };
            _channel.BasicConsume(queueName, true, consumer);
        }

        public async Task HandleAsync(LoadProductDetails message)
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
        private async Task SaveSupplierPriceList<T>(LoadProductDetails message) where T : IExcelModel, new()
        {
            var supplierSegment = new T().SupplierCode;
            var rows = await _priceListRepository.GetList();
            rows = rows.Where(x => x.SupplierCode == supplierSegment).ToList();
            _ = await _processAiWorkloads.RunSupplierProductsThroughAiOpsAsync(rows.ToArray());
        }
    }
}
