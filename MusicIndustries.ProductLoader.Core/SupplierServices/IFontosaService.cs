using System.Collections.Generic;
using System.Threading.Tasks;
using ProductLoader.DataContracts.SupplierPriceLists;

namespace MusicIndustries.ProductLoader.SupplierServices
{
    public interface IFontosaService
    {
        Task<IEnumerable<FontosaPriceList>> GetStockAsync();
        Task<IEnumerable<Brand>> GetBrandsAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Invoice> GetInvoiceAsync(string invoiceNumber, string transactionType);
        Task<IEnumerable<Transaction>> GetTransactionsAsync();
        Task<OrderResponse> PlaceOrderAsync(OrderRequest order);
        Task<dynamic> GetProductLinksAsync();
    }
}
