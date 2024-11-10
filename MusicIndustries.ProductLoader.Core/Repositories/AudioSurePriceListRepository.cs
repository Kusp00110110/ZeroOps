using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicIndustries.ProductLoader.Data.Context;
using ProductLoader.DataContracts.SupplierPriceLists;

namespace MusicIndustries.ProductLoader.Repositories
{
    public class AudioSurePriceListRepository : IRepository<AudioSurePriceList>
    {
        private readonly IServiceProvider _serviceProvider;
        public ApplicationDbContext context { get; set; }

        public AudioSurePriceListRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            using var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        }

        public async Task<IEnumerable<AudioSurePriceList>> GetList()
        {
             var scope = _serviceProvider.CreateScope();
                        context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AudioSurePriceListLoadItems.ToListAsync();
        }

        public async Task<AudioSurePriceList> GetItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AudioSurePriceListLoadItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AudioSurePriceList> GetItemBySlug(string slug)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AudioSurePriceListLoadItems.FirstOrDefaultAsync(x => x.FormattedItemNumber == slug);
        }

        public async Task<AudioSurePriceList> AddItem(AudioSurePriceList item)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.AudioSurePriceListLoadItems.Add(item);
            await context.SaveChangesAsync();
            return await context.AudioSurePriceListLoadItems.SingleAsync(x =>
                x.FormattedItemNumber == item.FormattedItemNumber);
        }

        public async Task<AudioSurePriceList> UpdateItem(AudioSurePriceList item)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.AudioSurePriceListLoadItems.Update(item);
            await context.SaveChangesAsync();
            return await context.AudioSurePriceListLoadItems.SingleAsync(x =>
                x.FormattedItemNumber == item.FormattedItemNumber);
        }

        public async Task<AudioSurePriceList> DeleteItem(int id)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var audioSurePriceList = await context.AudioSurePriceListLoadItems.FirstOrDefaultAsync(x => x.Id == id);
            context.AudioSurePriceListLoadItems.Remove(audioSurePriceList);
            await context.SaveChangesAsync();
            return audioSurePriceList;
        }

        public async Task<int> SaveChangesAsync()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<AudioSurePriceList, bool>> predicate)
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AudioSurePriceListLoadItems.AnyAsync(predicate);
        }

        public async Task<int> Count()
        {
            var scope = _serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.AudioSurePriceListLoadItems.CountAsync();
        }
    }
}
