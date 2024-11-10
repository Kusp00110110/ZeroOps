using System;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using MusicIndustries.ProductLoader.Data.MigrationHelpers;
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
    public class RunJsonLoadMessageSubscriber : ISubscriber<RunJsonLoadMessage>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IProcessExcelData _processExcelData;
        private readonly IPublisher<RunJsonLoadMessage> _retryPublisher;
        private readonly IFileImportProvider _fileImportProvider;
        private readonly ISeedingFromJsonHelper _seedingFromJsonHelper;
        private readonly IServiceProvider _serviceProvider;


        public RunJsonLoadMessageSubscriber(
            IProcessExcelData processExcelData,
            IPublisher<RunJsonLoadMessage> retryPublisher,
            IFileImportProvider fileImportProvider,
            ISeedingFromJsonHelper seedingFromJsonHelper,
            IServiceProvider serviceProvider)
        {
            _processExcelData = processExcelData;
            _retryPublisher = retryPublisher;
            _fileImportProvider = fileImportProvider;

            _seedingFromJsonHelper = seedingFromJsonHelper;
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare($"{nameof(RunJsonLoadMessage)}", ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, $"{nameof(RunJsonLoadMessage)}", "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received: {message}");
                var messageObject = JsonConvert.DeserializeObject<RunJsonLoadMessage>(message);
                await HandleAsync(messageObject!);
            };
            _channel.BasicConsume(queueName, true, consumer);
        }

        public async Task HandleAsync(RunJsonLoadMessage message)
        {
            // wait for the next retry
            await Task.Delay(message.NextRetryTime);

            // ACID transaction
            try
            {
                // Create a single DbContext instance here
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _seedingFromJsonHelper.context = context;
                switch (message.Supplier)
                {
                    case Supplier.AudioSure:
                         await _seedingFromJsonHelper.SeedAudiosure(true);
                        break;
                    case Supplier.RockitDistribution:
                        await _seedingFromJsonHelper.SeedRockitDistribution();
                        break;
                    case Supplier.MusicalDistributors:
                        await _seedingFromJsonHelper.SeedMusicalDistributors();
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
                if (message.RetryCount >= 3) throw;

                message.RetryCount++;
                message.NextRetryTime = message.NextRetryTime.Add(TimeSpan.FromSeconds(10));
                await _retryPublisher.PublishAsync(message);
            }
        }
    }
}
