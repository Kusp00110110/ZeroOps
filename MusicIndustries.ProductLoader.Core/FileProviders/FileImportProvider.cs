using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProductLoader.DataContracts.AppConfigurations;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.FileProviders
{
    public class FileImportProvider(FilerProviderConfiguration configuration) : IFileImportProvider
    {
        public async Task<Stream> GetFileStreamAsync(Supplier supplier)
        {
            return await OpenReadStreamFromPathAsync(supplier);
        }

        public async Task ArchiveProcessedFile(Supplier supplier)
        {
            await MoveStreamToArchive(supplier);
        }

        public async Task AddToFailedAudit(Supplier supplier)
        {
            await MoveStreamToError(supplier);
        }

        private async Task MoveStreamToError(Supplier supplier)
        {
            var importFolder = Path.Combine(configuration.Root, configuration.ImportFolderName, supplier.ToString());
            var errorFolder = Path.Combine(configuration.Root, configuration.ErrorFolderName, supplier.ToString());

            if (configuration.CreateIfNotExists)
            {
                if (!Directory.Exists(errorFolder))
                {
                    Directory.CreateDirectory(errorFolder);
                }
            }

            var importFiles = Directory.GetFiles(importFolder);
            // Should only be one file
            foreach (var importFile in importFiles)
            {
                var extension = Path.GetExtension(importFile);
                var processedAtFileName = Path.Combine(errorFolder, $"{supplier.ToString()}_{DateTime.Now:yyyyMMddHHmmss}.{extension}");
                var errorFile = Path.Combine(errorFolder, processedAtFileName);
                File.Move(importFile, errorFile);
            }
        }


        private async Task MoveStreamToArchive(Supplier supplier)
        {
            var importFolder = Path.Combine(configuration.Root, configuration.ImportFolderName, supplier.ToString());
            var archiveFolder = Path.Combine(configuration.Root, configuration.ArchiveFolderName, supplier.ToString());

            if (configuration.CreateIfNotExists)
            {
                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }
            }

            var importFiles = Directory.GetFiles(importFolder);
            // Should only be one file
            foreach (var importFile in importFiles)
            {
                var extension = Path.GetExtension(importFile);
                var processedAtFileName = Path.Combine(archiveFolder, $"{supplier.ToString()}_{DateTime.Now:yyyyMMddHHmmss}.{extension}");
                var archiveFile = Path.Combine(archiveFolder, processedAtFileName);
                File.Move(importFile, archiveFile);
            }
        }

        private async Task<Stream> OpenReadStreamFromPathAsync(Supplier supplier)
        {
            var importFolder = Path.Combine(configuration.Root, configuration.ImportFolderName, supplier.ToString());

            if (configuration.CreateIfNotExists)
            {
                if (!Directory.Exists(importFolder))
                {
                    Directory.CreateDirectory(importFolder);
                }
            }

            var file = Directory.GetFiles(importFolder).SingleOrDefault();

            if (string.IsNullOrEmpty(file))
            {
                return null;
            }

            await using var fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
