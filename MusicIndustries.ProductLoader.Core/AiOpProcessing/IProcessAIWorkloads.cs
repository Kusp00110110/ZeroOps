using System.Threading.Tasks;
using ProductLoader.DataContracts.OpenAiApi;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.AiOpProcessing
{
    public interface IProcessAiWorkloads
    {
        Task<AiOpsProductSchemaResultModel> RunSupplierProductThroughAiOpsAsync(PriceListRow priceListRows);
        Task<AiOpsProductSchemaResultModel[]> RunSupplierProductsThroughAiOpsAsync(PriceListRow[] priceListRows);

        Task<TSchema> UseAiToMatchImageWithProduct<TSchema>(string question)
            where TSchema : class, new();

    }
}
