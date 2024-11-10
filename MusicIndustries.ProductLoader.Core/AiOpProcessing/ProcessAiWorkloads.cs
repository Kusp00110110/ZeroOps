using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MusicIndustries.ProductLoader.Extensions.Extensions;
using MusicIndustries.ProductLoader.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using OpenAI.Chat;
using ProductLoader.DataContracts.AppConfigurations;
using ProductLoader.DataContracts.DocumentStore;
using ProductLoader.DataContracts.OpenAiApi;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.AiOpProcessing
{
    public class ProcessAiWorkloads(
        IRepository<DocumentStorgageItem> aiOpsRepositoryFileCache,
        IRepository<AiOpsProductSchemaResultModel> aiOpsRepository,
        AiOperationsConfiguration aiOperationsConfiguration
    ) : IProcessAiWorkloads
    {

        public async Task<AiOpsProductSchemaResultModel> RunSupplierProductThroughAiOpsAsync(PriceListRow priceListRows)
        {
            return await ChatWithAiAsync(priceListRows, priceListRows.SupplierCode);
        }
        public async Task<AiOpsProductSchemaResultModel[]> RunSupplierProductsThroughAiOpsAsync(PriceListRow[] priceListRows)
        {
            var chunks = priceListRows.ChunkBy(4);
            var result = new List<AiOpsProductSchemaResultModel>();
            foreach (var chunk in chunks)
            {
                #if DEBUG
                // Dont run parallel in debug mode
                foreach (var item in chunk)
                {
                    var itemResult = await ChatWithAiAsync(item, item.SupplierCode);
                    result.Add(itemResult);
                }
                #else
                var tasks = chunk.Select(x => ChatWithAiAsync(x, x.SupplierCode));
                var chunkResult = await Task.WhenAll(tasks);
                result.AddRange(chunkResult);
                #endif
            }
            return result.ToArray();
        }
        public async Task<TSchema> UseAiToMatchImageWithProduct<TSchema>(string question) where TSchema : class, new()
        {
            throw new NotImplementedException();
        }

        private async Task<AiOpsProductSchemaResultModel> ChatWithAiAsync(
            PriceListRow product, string supplierCode)
        {

            // dont call the AI if the item already exists
            var exists = await aiOpsRepository.GetItemBySlug(product.ItemNumber);
            if(exists != null)
            {
                return exists;
            }

            var (json, schemaObject, schema) =
                await GenerateSchema(product);
            var bytes = Encoding.UTF8.GetBytes(schema.JsonContent);
            var binaryData = new BinaryData(bytes);
            OpenAIClient client = new(aiOperationsConfiguration.ApiKey);
#pragma warning disable OPENAI001
            // var batchClient = client.GetBatchClient();
            // TODO - Implement batch processing after its not experimental
            // read more here https://www.nuget.org/packages/OpenAI/2.0.0
#pragma warning restore OPENAI001

            var systemTemplate = await File.ReadAllTextAsync("PromptTemplates/ProductInfo/ProductInfoSystem.md");
            var assistantTemplate = await File.ReadAllTextAsync("PromptTemplates/ProductInfo/ProductInfoAssistant.md");
            var userTemplate = await File.ReadAllTextAsync("PromptTemplates/ProductInfo/ProductInfoUser.md");
            var userPrompt = string.Format(userTemplate, json);

            List<ChatMessage> messages =
            [
                new SystemChatMessage(systemTemplate),
                new AssistantChatMessage(assistantTemplate),
                new UserChatMessage(userPrompt)
            ];
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    "question_answering",
                    binaryData,
                    jsonSchemaIsStrict: true)
            };
            try
            {
                var stopwatch = Stopwatch.StartNew();
                var chatClient = client.GetChatClient(aiOperationsConfiguration.ModelName);
                stopwatch.Start();
                var completion = await chatClient.CompleteChatAsync(messages, options);

                var usage = completion.Value.Usage;
                var dynamicUsage = new UsageTrackingModel
                {
                    InputTokens = usage.InputTokenCount,
                    InputCostPerMillion = 0.150m,
                    TotalInputCost = usage.InputTokenCount * (0.150m / 1000000),
                    TotalInputCostRand = usage.InputTokenCount * (0.150m / 1000000) *
                                         aiOperationsConfiguration.RandToUsdExchangeRate,
                    OutputTokens = usage.OutputTokenCount,
                    OutputCostPerMillion = 0.060m,
                    TotalOutputCost = usage.OutputTokenCount * (0.060m / 1000000),
                    TotalOutputCostRand = usage.OutputTokenCount * (0.060m / 1000000) *
                                          aiOperationsConfiguration.RandToUsdExchangeRate
                };

                stopwatch.Stop();
                Console.WriteLine($"AIOPS processing completed in {stopwatch.ElapsedMilliseconds}ms");
                var jsonDocument = JsonDocument.Parse(completion.Value.Content[0].Text);
                var resultModel = JsonConvert.DeserializeObject<AiOpsProductSchemaResultModelDto>(jsonDocument.RootElement.ToString());
                if (resultModel is not null)
                {
                    return await aiOpsRepository.AddItem(new AiOpsProductSchemaResultModel
                    {
                        ItemNumber = product.ItemNumber,
                        ProductManufacturerBrandName = resultModel.ProductManufacturerBrandName,
                        ProductName = resultModel.ProductName,
                        ProductDescription = resultModel.ProductDescription,
                        ProductCategoriesJson = JsonConvert.SerializeObject(resultModel.ProductCategoriesAndSubCategories),
                        ProductCategoriesAndSubCategories = resultModel.ProductCategoriesAndSubCategories,
                        ProductSpecification = resultModel.ProductSpecification,
                        ProductLengthMeters = resultModel.ProductLengthMeters,
                        ProductWidthMeters = resultModel.ProductWidthMeters,
                        ProductHeightMeters = resultModel.ProductHeightMeters,
                        ProductWeightKg = resultModel.ProductWeightKg,
                        DuckDuckGoImageApiSearchString = resultModel.DuckDuckGoImageApiSearchString
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return new AiOpsProductSchemaResultModel();
        }

        private async Task<(string json, string schemaObject, DocumentStorgageItem? schema)> GenerateSchema(
            PriceListRow product)
        {
            var typeName = nameof(AiOpsProductSchemaResultModelDto);
            var json = JsonConvert.SerializeObject(product, Formatting.Indented);
            var schemaObject = JsonConvert.SerializeObject(new AiOpsProductSchemaResultModelDto(), Formatting.Indented);
            var generator = new JSchemaGenerator()
            {
                // Ensures that the enum values are generated as strings
                GenerationProviders = { new StringEnumGenerationProvider() }
            };
            var schema = (await aiOpsRepositoryFileCache
                    .GetList())
                .FirstOrDefault(x => x.DocumentName == $"{typeName}-AIOPS-JsonSchema");

            if (schema is not null) return (json, schemaObject, schema);

            var generatedSchema = generator.Generate(typeof(AiOpsProductSchemaResultModelDto));
            generatedSchema.AllowAdditionalProperties = false;
            var documentStoreItem = new DocumentStorgageItem()
            {
                DocumentName = $"{typeName}-AIOPS-JsonSchema",
                JsonContent = generatedSchema.ToString()
            };
            var item = await aiOpsRepositoryFileCache
                .AddItem(documentStoreItem);
            schema = await aiOpsRepositoryFileCache.GetItem(item.Id);

            return (json, schemaObject, schema);
        }
    }
}
