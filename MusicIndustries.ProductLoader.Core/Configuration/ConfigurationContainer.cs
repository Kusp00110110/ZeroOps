using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Configuration;
using ProductLoader.DataContracts.AppConfigurations;

namespace MusicIndustries.ProductLoader.Configuration
{
    public static class ConfigurationContainer
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            // Add Iconfiguration from namespace Microsoft.Extensions.Configuration
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .AddEnvironmentVariables()
                .Build();

            // Configuration Model Binding

            var cacheConfig = new CacheConfiguration();
            configurationBuilder.GetSection("CacheConfiguration").Bind(cacheConfig);

            var aiOpsConfig = new AiOperationsConfiguration();
            configurationBuilder.GetSection("AiOperationsConfiguration").Bind(aiOpsConfig);

            var fileProviderConfig = new FilerProviderConfiguration();
            configurationBuilder.GetSection("FileProviderConfiguration").Bind(fileProviderConfig);
            services.AddSingleton<IConfiguration>(configurationBuilder);
            services.AddSingleton(cacheConfig);
            services.AddSingleton(aiOpsConfig);
            services.AddSingleton(fileProviderConfig);
            return services;
        }
    }
}
