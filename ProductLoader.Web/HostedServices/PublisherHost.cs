using MusicIndustries.ProductLoader.FileProviders;
using MusicIndustries.ProductLoader.MessageHandling.Infrastructure;
using MusicIndustries.ProductLoader.MessageHandling.Messages;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace ProductLoader.Web.HostedServices
{
    public class PublisherHost : BackgroundService
    {

        IPublisher<RunJsonLoadMessage> _jsonLoadPublisher;
        IPublisher<LoadSupplierPriceList> _loadSupplierPriceListPublisher;
        IPublisher<LoadProductDetails> _loadProductDetailsPublisher;

        private readonly ILogger<PublisherHost> _logger;
        public PublisherHost(ILogger<PublisherHost> logger,
            IFileImportProvider fileImportProvider,
            IPublisher<RunJsonLoadMessage> jsonLoadPublisher,
            IPublisher<LoadSupplierPriceList> loadSupplierPriceListPublisher, IPublisher<LoadProductDetails> loadProductDetailsPublisher)
        {
            _logger = logger;
            _jsonLoadPublisher = jsonLoadPublisher;
            _loadSupplierPriceListPublisher = loadSupplierPriceListPublisher;
            _loadProductDetailsPublisher = loadProductDetailsPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkerService running at: {time}", DateTimeOffset.Now);
            bool runOnce = true;
            // await _jsonLoadPublisher.PublishAsync(new RunJsonLoadMessage
            // {
            //     Supplier = Supplier.MusicalDistributors,
            // });

            var nextRun = DateTime.Now.AddSeconds(10);
            while (!stoppingToken.IsCancellationRequested)
            {
                //every 5 minutes run supplier price list loads
                if (DateTime.Now > nextRun)
                {
                    // await _loadSupplierPriceListPublisher.PublishAsync(new LoadSupplierPriceList
                    // {
                    //     Supplier = Supplier.AudioSure,
                    // });
                    // await _loadSupplierPriceListPublisher.PublishAsync(new LoadSupplierPriceList
                    // {
                    //     Supplier = Supplier.RockitDistribution,
                    // });
                    await _loadProductDetailsPublisher.PublishAsync(new LoadProductDetails
                    {
                        Supplier = Supplier.AudioSure,
                    });

                    await _loadProductDetailsPublisher.PublishAsync(new LoadProductDetails
                    {
                        Supplier = Supplier.RockitDistribution,
                    });

                    nextRun = DateTime.Now.AddDays(1);
                }
            }

            _logger.LogInformation("WorkerService is stopping.");
        }
    }
}
