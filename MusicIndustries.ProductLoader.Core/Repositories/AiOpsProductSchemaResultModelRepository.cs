using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using ProductLoader.DataContracts.OpenAiApi;

namespace MusicIndustries.ProductLoader.Repositories
{
    public class AiOpsProductSchemaResultModelRepository
        : IRepository<AiOpsProductSchemaResultModel>
    {
        private readonly IServiceProvider _serviceProvider;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        public AiOpsProductSchemaResultModelRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        }
        public async Task<IEnumerable<AiOpsProductSchemaResultModel>> GetList()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AiOpsProductSchemaResultModels.ToListAsync();
        }

        public async Task<AiOpsProductSchemaResultModel> GetItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AiOpsProductSchemaResultModels.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AiOpsProductSchemaResultModel> GetItemBySlug(string slug)
        {
            try
            {
                await _semaphore.WaitAsync();
                var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                return await context.AiOpsProductSchemaResultModels.FirstOrDefaultAsync(x => x.ItemNumber == slug);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<AiOpsProductSchemaResultModel> AddItem(AiOpsProductSchemaResultModel item)
        {
            try
            {
                await _semaphore.WaitAsync();
                var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var existingItems = context.AiOpsProductSchemaResultModels.Where(x => x.ItemNumber == item.ItemNumber);
                if (existingItems.Count() > 1)
                {
                    context.AiOpsProductSchemaResultModels.RemoveRange(existingItems);
                    await context.SaveChangesAsync();
                }
                var existingItem = await context.AiOpsProductSchemaResultModels.FirstOrDefaultAsync(x => x.ItemNumber == item.ItemNumber);

                await context.AiOpsProductSchemaResultModels.AddAsync(item);
                await context.SaveChangesAsync();
                return await context.AiOpsProductSchemaResultModels.SingleAsync(x => x.ItemNumber == item.ItemNumber);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _semaphore.Release();
            }


        }

        public async Task<AiOpsProductSchemaResultModel> UpdateItem(AiOpsProductSchemaResultModel item)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.AiOpsProductSchemaResultModels.Update(item);
            await context.SaveChangesAsync();
            return await context.AiOpsProductSchemaResultModels.SingleAsync(x => x.ItemNumber == item.ItemNumber);
        }

        public async Task<AiOpsProductSchemaResultModel> DeleteItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var item = await context.AiOpsProductSchemaResultModels.FirstOrDefaultAsync(x => x.Id == id);
            context.AiOpsProductSchemaResultModels.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<int> SaveChangesAsync()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<AiOpsProductSchemaResultModel, bool>> predicate)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AiOpsProductSchemaResultModels.AnyAsync(predicate);
        }

        public async Task<int> Count()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AiOpsProductSchemaResultModels.CountAsync();
        }
    }
}
