using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using ProductLoader.DataContracts.DocumentStore;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.Repositories
{
    public class DocumentStoreRepository : IRepository<DocumentStorgageItem>
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentStoreRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            using var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public ApplicationDbContext context { get; set; }
        public async Task<IEnumerable<DocumentStorgageItem>> GetList()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.DocumentStoreItems.ToListAsync();
        }

        public async Task<DocumentStorgageItem> GetItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.DocumentStoreItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DocumentStorgageItem> GetItemBySlug(string slug)
        {
             var scope = _serviceProvider.CreateScope();
                        context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.DocumentStoreItems.FirstOrDefaultAsync(x => x.DocumentName == slug);
        }

        public async Task<DocumentStorgageItem> AddItem(DocumentStorgageItem item)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.DocumentStoreItems.Add(item);
            await context.SaveChangesAsync();
            return await context.DocumentStoreItems.SingleAsync(x => x.DocumentName == item.DocumentName);
        }

        public async Task<DocumentStorgageItem> UpdateItem(DocumentStorgageItem item)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.DocumentStoreItems.Update(item);
            await context.SaveChangesAsync();
            return await context.DocumentStoreItems.SingleAsync(x => x.DocumentName == item.DocumentName);
        }


        public async Task<DocumentStorgageItem> DeleteItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var item = await context.DocumentStoreItems.FirstOrDefaultAsync(x => x.Id == id);
            context.DocumentStoreItems.Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<int> SaveChangesAsync()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<DocumentStorgageItem, bool>> predicate)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return context.DocumentStoreItems.Any(predicate);
        }

        public async Task<int> Count()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return context.DocumentStoreItems.Count();
        }
    }
}
