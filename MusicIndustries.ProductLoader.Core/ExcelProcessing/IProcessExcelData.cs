using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ProductLoader.DataContracts.SupplierPriceLists;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.ExcelProcessing
{
    public interface IProcessExcelData
    {
        Task<T[]> ReadExcelFileAsync<T>(Stream fileStream) where T : IExcelModel, new();
        Task<Stream> WriteExcelModelAsync<TResult>(IEnumerable<TResult> data)
            where TResult : class;
    }
}
