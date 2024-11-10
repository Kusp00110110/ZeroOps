using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.Repositories
{
    public class PriceListRowRepository : IRepository<PriceListRow>
    {
        public ApplicationDbContext context { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public PriceListRowRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            using var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public async Task<IEnumerable<PriceListRow>> GetList()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.PriceListLoadItems.ToListAsync();
        }

        public async Task<PriceListRow> GetItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.PriceListLoadItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PriceListRow> GetItemBySlug(string slug)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.PriceListLoadItems.FirstOrDefaultAsync(x => x.ItemNumber == slug);
        }

        public async Task<PriceListRow> AddItem(PriceListRow item)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // If the item exists add it or update
            var existingItems = context.PriceListLoadItems.Where(x => x.ItemNumber == item.ItemNumber);
            if(existingItems.Count() > 1)
            {
                context.PriceListLoadItems.RemoveRange(existingItems);
                await context.SaveChangesAsync();
            }
            var existingItem = await context.PriceListLoadItems.FirstOrDefaultAsync(x => x.ItemNumber == item.ItemNumber);
            if (existingItem != null)
            {
                existingItem.Brand = item.Brand ?? existingItem.Brand;
                existingItem.Description = item.Description ?? existingItem.Description;
                existingItem.RecommendedRetailPrice = item.RecommendedRetailPrice;
                existingItem.QuantityOnHand = item.QuantityOnHand;
                context.PriceListLoadItems.Update(existingItem);
                await context.SaveChangesAsync();
            }
            else if (!string.IsNullOrEmpty(item.Brand))
            {
                context.PriceListLoadItems.Add(item);
                await context.SaveChangesAsync();
                return await context.PriceListLoadItems.SingleAsync(x => x.ItemNumber == item.ItemNumber);
            }
            return await context.PriceListLoadItems.FirstOrDefaultAsync(x => x.ItemNumber == item.ItemNumber);
        }

        public async Task<PriceListRow> UpdateItem(PriceListRow item)
        {
            context.PriceListLoadItems.Update(item);
            var rowId = await context.SaveChangesAsync();
            return await context.PriceListLoadItems.SingleAsync(x => x.ItemNumber == item.ItemNumber);
        }

        public async Task<PriceListRow> DeleteItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var priceListRow = context.PriceListLoadItems.FirstOrDefault(x => x.Id == id);
            context.PriceListLoadItems.Remove(priceListRow);
            await context.SaveChangesAsync();
            return priceListRow;
        }

        public async Task<int> SaveChangesAsync()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<PriceListRow, bool>> predicate)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.PriceListLoadItems.AnyAsync(predicate);
        }

        public async Task<int> Count()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.PriceListLoadItems.CountAsync();
        }


    }
}
