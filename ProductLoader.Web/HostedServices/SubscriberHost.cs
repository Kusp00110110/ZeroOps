using MusicIndustries.ProductLoader.FileProviders;
using MusicIndustries.ProductLoader.MessageHandling.Infrastructure;
using MusicIndustries.ProductLoader.MessageHandling.Messages;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace ProductLoader.Web.HostedServices
{
    public class SubscriberHost : BackgroundService
    {

        private List<dynamic> _subscribers = new();
        private readonly ILogger<SubscriberHost> _logger;
        public SubscriberHost(ILogger<SubscriberHost> logger,
            ISubscriber<RunJsonLoadMessage> jsonLoadSubscriber,
            ISubscriber<LoadSupplierPriceList> loadSupplierPriceListSubscriber,
            ISubscriber<LoadProductDetails> loadProductDetailsSubscriber)
        {
            _logger = logger;
            _subscribers.AddRange([jsonLoadSubscriber, loadSupplierPriceListSubscriber, loadProductDetailsSubscriber]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkerService running at: {time}", DateTimeOffset.Now);
            bool runOnce = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                // load all the subscribers
                if (runOnce)
                {
                    foreach (var subscriber in _subscribers)
                    {
                        Console.WriteLine("Registered subscriber: " + subscriber.GetType().Name);
                    }
                    runOnce = false;
                }

            }
            _logger.LogInformation("WorkerService is stopping.");
        }
    }
}
