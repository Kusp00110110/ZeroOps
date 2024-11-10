using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Repositories;
using ProductLoader.DataContracts.DocumentStore;
using ProductLoader.DataContracts.OpenAiApi;
using ProductLoader.DataContracts.SupplierPriceLists;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.Configuration
{
    public static class RepositoryContainer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository<AiOpsProductSchemaResultModel>,
                AiOpsProductSchemaResultModelRepository>();
            services.AddTransient<IRepository<DocumentStorgageItem>,
                DocumentStoreRepository>();
            services.AddTransient<IRepository<PriceListRow>,
                PriceListRowRepository>();
            services.AddTransient<IRepository<AudioSurePriceList>,
                AudioSurePriceListRepository>();

            return services;
        }
    }
}
