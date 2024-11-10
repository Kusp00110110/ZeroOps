using System;
using System.Text;
using System.Threading.Tasks;
using MusicIndustries.ProductLoader.MessageHandling.Infrastructure;
using MusicIndustries.ProductLoader.MessageHandling.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MusicIndustries.ProductLoader.MessageHandling.Publishers
{
    public class RunJsonLoadMessagePublisher : IPublisher<RunJsonLoadMessage>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RunJsonLoadMessagePublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare($"{nameof(RunJsonLoadMessage)}", ExchangeType.Fanout); // Fanout for pub/sub
        }

        public async Task PublishAsync(RunJsonLoadMessage message)
        {
            var jsonMessageBody = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessageBody);
            _channel.BasicPublish($"{nameof(RunJsonLoadMessage)}", "", null, body);
            Console.WriteLine($"[x] Sent: {message}");
        }
    }
}
