using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ExcelDataReader;
using OfficeOpenXml;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.ExcelProcessing
{
    public class ProcessExcelData : IProcessExcelData
    {
        public async Task<T[]> ReadExcelFileAsync<T>(Stream fileStream) where T : IExcelModel, new()
        {
            var loadedData = new List<T>();
            var loaderModel = new T();
            var headerRow = (int)loaderModel.HeaderRowNumber;
            var readStartRow = loaderModel.HasHeader ? headerRow + 1 : headerRow;
            var readColumnStart = (int)loaderModel.HeaderStartColumn - 1;
            var readColumnEnd = (int)loaderModel.HeaderEndColumn - 1;

            using var reader = ExcelReaderFactory.CreateReader(fileStream);
            using var dataSet = reader.AsDataSet();
            using var dataTable = dataSet.Tables[loaderModel.SheetName];

            for (var i = readStartRow; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                var headerRowItems = dataTable.Rows[headerRow - 1].ItemArray;
                var stockItem = new T();

                for (var colIndex = readColumnStart; colIndex <= readColumnEnd; colIndex++)
                {
                    var columnName = headerRowItems[colIndex]?.ToString()?.Replace(" ", "");
                    var columnValue = row[colIndex]?.ToString();
                    if (!string.IsNullOrEmpty(columnValue))
                    {
                        stockItem.MapColumn(columnName, columnValue);
                    }
                }
                loadedData.Add(stockItem);
            }

            return loadedData.ToArray();
        }


        public async Task<Stream> WriteExcelModelAsync<TResult>(IEnumerable<TResult> data) where TResult : class
        {
            var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(data, true);
            package.Save();
            stream.Position = 0;
            return stream;
        }
    }
}
