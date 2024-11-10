using System.Threading.Tasks;
using MusicIndustries.ProductLoader.Data.Context;

namespace MusicIndustries.ProductLoader.Data.MigrationHelpers
{
    public interface ISeedingFromJsonHelper
    {
        ApplicationDbContext context { get; set; }
        Task SeedData();
        Task RollbackSeed();
        Task SeedAudiosure(bool b);
        Task SeedRockitDistribution();
        Task SeedMusicalDistributors();
        string EscapeSingleQuotes(string input);
        Task InsertAiOpsWithImageData(SeedingFromJsonHelper.MusicIndustries item);
    }
}
