using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using Newtonsoft.Json;
using ProductLoader.DataContracts.ImageApi;
using ProductLoader.DataContracts.OpenAiApi;
using ProductLoader.DataContracts.SupplierPriceLists;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.Data.MigrationHelpers
{
    public class SeedingFromJsonHelper : ISeedingFromJsonHelper
    {
        public ApplicationDbContext context { get; set; }
        public async Task SeedData()
        {
            await SeedAudiosure();
            await SeedRockitDistribution();
            await SeedMusicalDistributors();
        }
        public async Task RollbackSeed()
        {
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE AudioSurePriceListLoadItems");
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE RockitPriceListLoadItems");
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE MdPriceListLoadItems");
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE AiOpsProductSchemaResultModels");
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE ImageApiResponses");
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE PriceListLoadItems");
        }

        public async Task SeedAudiosure(bool cleanFirst = false)
        {

            if (cleanFirst)
            {
                await RollbackSeed();
            }

            Console.WriteLine("Seeding Audiosure");
            // AudioSurePriceList
            var jsonData = await File.ReadAllTextAsync("SeedData/AUDIOSURE.json");
            var audioSurePriceList = JsonConvert.DeserializeObject<List<PriceListRow>>(jsonData);
            var loadItems = audioSurePriceList.Where(x => x.Brand != null && x.ItemNumber != null).ToArray();
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE PriceListLoadItems");
            await context.PriceListLoadItems.AddRangeAsync(loadItems);
            await context.SaveChangesAsync();

            var aiData = await File.ReadAllTextAsync("SeedData/AUDIOSURE-MusicIndustries.json");
            var audioSureAiOpsResult = JsonConvert.DeserializeObject<List<MusicIndustries>>(aiData);
            foreach(var item in audioSureAiOpsResult)
            {
                await InsertAiOpsWithImageData(item);
            };

        }

        public async Task SeedRockitDistribution()
        {

            Console.WriteLine("Seeding Rockit Distribution");
            // AudioSurePriceList
            var jsonData = await File.ReadAllTextAsync("SeedData/ROCKIT.json");
            var pricelistItems = JsonConvert.DeserializeObject<List<PriceListRow>>(jsonData);

            var loadItems = pricelistItems.Where(x => x.Brand != null && x.ItemNumber != null).ToArray();
            await context.PriceListLoadItems.AddRangeAsync(loadItems);
            await context.SaveChangesAsync();

            // AudioSureAiOpsResult
            var aiData = await File.ReadAllTextAsync("SeedData/ROCKIT-MusicIndustries.json");
            var audioSureAiOpsResult = JsonConvert.DeserializeObject<List<MusicIndustries>>(aiData);
            // audioSureAiOpsResult.ForEach(async item => {
            //     await InsertAiOpsWithImageData(item);
            // });
        }


        public async Task SeedMusicalDistributors()
        {
            Console.WriteLine("Seeding Musical Distributors");

            // AudioSurePriceList
            var jsonData = await File.ReadAllTextAsync("SeedData/MUSICALDISTRIBUTORS.json");
            var audioSurePriceList = JsonConvert.DeserializeObject<List<PriceListRow>>(jsonData);

            var loadItems = audioSurePriceList.Where(x => x.Brand != null && x.ItemNumber != null).ToArray();
            context.PriceListLoadItems.AddRange(loadItems);
            await context.SaveChangesAsync();

            // AudioSureAiOpsResult
            var aiData = await File.ReadAllTextAsync("SeedData/MUSICALDISTRIBUTORS-MusicIndustries.json");
            var audioSureAiOpsResult = JsonConvert.DeserializeObject<List<MusicIndustries>>(aiData);
            // foreach (var item in audioSureAiOpsResult)
            // {
            //     await InsertAiOpsWithImageData(item);
            // }
        }

        public class MusicIndustries
        {
            public AiOpsProductSchemaResultModel AiOpsProductSchemaResultModel { get; set; }

            public List<ImageApiResponse> ImageApiLoaderModelResults { get; set; }
        }

        #region Helpers

        public string EscapeSingleQuotes(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Replace("'", "''");
        }

        public async Task InsertAiOpsWithImageData(MusicIndustries item)
        {
            var product = item.AiOpsProductSchemaResultModel;
            var images = item.ImageApiLoaderModelResults;
            await context.AiOpsProductSchemaResultModels.AddAsync(product);
            await context.SaveChangesAsync();
            var savedProduct = await context.AiOpsProductSchemaResultModels
                .Where(x => x.ItemNumber == product.ItemNumber)
                .FirstOrDefaultAsync();
            images.ForEach(image => {
                image.AiOpsProductSchemaResultModelId = savedProduct.Id;
            });
            await context.ImageApiResponses.AddRangeAsync(images);
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
