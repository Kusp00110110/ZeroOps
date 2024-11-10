using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.MessageHandling.Infrastructure;
using MusicIndustries.ProductLoader.MessageHandling.Messages;
using MusicIndustries.ProductLoader.MessageHandling.Publishers;
using MusicIndustries.ProductLoader.MessageHandling.Subscribers;

namespace MusicIndustries.ProductLoader.Configuration
{
    public static class MessageBusContainer
    {
        public static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            services.AddTransient<IPublisher<LoadSupplierPriceList>, LoadSupplierPriceListPublisher>();
            services.AddTransient<IPublisher<RunJsonLoadMessage>, RunJsonLoadMessagePublisher>();
            services.AddTransient<IPublisher<LoadProductDetails>, LoadProductDetailsPublisher>();
            return services;

        }
        public static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            services.AddTransient<ISubscriber<LoadSupplierPriceList>, LoadSupplierPriceListSubscriber>();
            services.AddTransient<ISubscriber<RunJsonLoadMessage>, RunJsonLoadMessageSubscriber>();
            services.AddTransient<ISubscriber<LoadProductDetails>, LoadProductDetailsSubscriber>();
            return services;
        }
    }
}
