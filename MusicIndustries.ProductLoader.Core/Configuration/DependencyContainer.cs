using Elsa.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.AiOpProcessing;
using MusicIndustries.ProductLoader.Data.Configuration;
using MusicIndustries.ProductLoader.ExcelProcessing;
using MusicIndustries.ProductLoader.FileProviders;
using MusicIndustries.ProductLoader.Notifications;
using MusicIndustries.ProductLoader.ProductImages;
using MusicIndustries.ProductLoader.SupplierServices;

namespace MusicIndustries.ProductLoader.Configuration
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddProductsLoaderDependencies(this IServiceCollection services)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // Add App Configuration
            services.AddApplicationConfiguration();

            // Add Ef Core and Repository Pattern
            services.AddProductLoaderDataService();
            services.AddRepositories();


            // Add Message Bus
            services.AddPublishers();
            services.AddSubscribers();

            // Add Services
            services.AddTransient<IProcessAiWorkloads, ProcessAiWorkloads>();
            services.AddTransient<IProcessExcelData, ProcessExcelData>();
            services.AddTransient<IFileImportProvider, FileImportProvider>();
            services.AddTransient<IProcessNotifications, ProcessNotifications>();
            services.AddTransient<IProcessProductImages, ProcessProductImages>();

            // Supplier Services
            services.AddTransient<IFontosaService, FontosaService>();


            // Adds HttpClient
            services.AddHttpClient<IProcessNotifications>();
            services.AddHttpClient<IProcessProductImages>();
            services.AddHttpClient<IFontosaService>();
            return services;
        }

    }
}
