using Microsoft.EntityFrameworkCore;
using ProductLoader.DataContracts.AppConfigurations;
using ProductLoader.DataContracts.DocumentStore;
using ProductLoader.DataContracts.ImageApi;
using ProductLoader.DataContracts.OpenAiApi;
using ProductLoader.DataContracts.SupplierPriceLists;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.Data.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        // Define your DbSets (tables) here
        public DbSet<PriceListRow> PriceListLoadItems { get; set; }
        public DbSet<DocumentStorgageItem> DocumentStoreItems { get; set; }
        public DbSet<AiOpsProductSchemaResultModel> AiOpsProductSchemaResultModels { get; set; }
        public DbSet<AudioSurePriceList> AudioSurePriceListLoadItems { get; set; }
        public DbSet<MdPriceList> MdPriceListLoadItems { get; set; }
        public DbSet<RockitPriceList> RockitPriceListLoadItems { get; set; }

        public DbSet<ImageApiResponse> ImageApiResponses { get; set; }

        public DbSet<AiOperationsConfiguration> AiOperationsConfiguration { get; set; }
        public DbSet<CacheConfiguration> CacheConfiguration { get; set; }


    }
}
