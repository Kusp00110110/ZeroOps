using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using MusicIndustries.ProductLoader.Data.MigrationHelpers;

namespace MusicIndustries.ProductLoader.Data.Configuration
{
    public static class ProductLoaderDataServiceContainer
    {
        public static IServiceCollection AddProductLoaderDataService(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            // Add Migration Helpers
            services.AddTransient<ISeedingFromJsonHelper, SeedingFromJsonHelper>();
            return services;
        }
    }
}
