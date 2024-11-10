using MusicIndustries.ProductLoader.Configuration;

namespace ProductLoader.Web.Configuration
{
    public static class LoaderWebContainer
    {
        public static IServiceCollection AddLoaderWebDependencies(this IServiceCollection services)
        {
            services.AddProductsLoaderDependencies();
            return services;
        }
    }
}
