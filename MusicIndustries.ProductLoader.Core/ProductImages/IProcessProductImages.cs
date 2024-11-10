using System.Threading.Tasks;
using ProductLoader.DataContracts.ImageApi;

namespace MusicIndustries.ProductLoader.ProductImages
{
    public interface IProcessProductImages
    {
        Task<ImageApiResponse[]?> SearchDuckDuckGoAsync(string keywords);
        Task<bool> CheckImageExistsAsync(string url);
        Task<ImageApiResponse[]> SearchAsync(string keywords);
    }
}
