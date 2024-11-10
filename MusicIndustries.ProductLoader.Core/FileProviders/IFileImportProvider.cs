using System.IO;
using System.Threading.Tasks;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.FileProviders
{
    public interface IFileImportProvider
    {
        Task<Stream> GetFileStreamAsync(Supplier supplier);

        Task ArchiveProcessedFile(Supplier supplier);

        Task AddToFailedAudit(Supplier supplier);
    }
}
