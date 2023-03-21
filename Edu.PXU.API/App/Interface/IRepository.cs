using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Edu.PXU.API.App.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T t);
        Task CreateAsync(ICollection<T> t);
        Task<T?> Update(T t, object key);
        void Delete(string id);
        void Remove(T t);
        void RemoveAll(ICollection<T> t);
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllIncludeAsync(Expression<Func<T, bool>> match, Func<IQueryable<T>, IIncludableQueryable<T, object>> include);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> match);
        IEnumerable<T> GetAll<TProperty>(Expression<Func<T, bool>> match,
                      int pageSize,
                      int pageIndex,
                      out int total,
                      Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                      bool disableTracking = true,
                      bool ignoreQueryFilters = false);
    }
}
