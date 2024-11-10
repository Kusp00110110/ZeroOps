using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MusicIndustries.ProductLoader.Data.Context;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.Repositories
{
    public interface IRepository<T>
    {
        // EF Core Repository Pattern
        Task<IEnumerable<T>> GetList();

        Task<T> GetItem(int id);

        Task<T> GetItemBySlug(string slug);

        Task<T> AddItem(T item);

        Task<T> UpdateItem(T item);

        Task<T> DeleteItem(int id);

        Task<int> SaveChangesAsync();

        Task<bool> Any(Expression<Func<T, bool>> predicate);

        Task<int> Count();

        }
}
