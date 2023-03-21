using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Edu.PXU.API.App
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PXUDBContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(PXUDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            return t;
        }

        public void Delete(string id)
        {
            var ent = _context.Set<T>().Find(id);
            _context.Remove(ent);
            
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        

        public IEnumerable<T> GetAll<TProperty>(Expression<Func<T, bool>> match,
             int pageSize, int pageIndex,
             out int total,
             Func<IQueryable<T>,
             IIncludableQueryable<T, object>>? include = null,
             bool disableTracking = true,
             bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (match != null)
            {
                query = query.Where(match);
            }
            if (include != null)
            {
                query = include(query);
            }
            total = query.Count();

            if (pageSize > 0)
            {
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }

            return query.ToList();
        }

        public async  Task<T?> Update(T t, object key)
        {
            T? exist = _context.Set<T>().Find(key);

            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
            }

            return exist;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> match)
        {
            IQueryable<T> query = _dbSet;

            if (match != null)
            {
                query = query.Where(match);
            }

            return query.ToList();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            
            return await _context.Set<T>().ToListAsync();
        }

        public async Task CreateAsync(ICollection<T> t)
        {
            await _context.Set<T>().AddRangeAsync(t);
        }

        public async Task<IEnumerable<T>> GetAllIncludeAsync(Expression<Func<T, bool>> match, Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            IQueryable<T> query = _dbSet;

            if (match != null)
            {
                query = query.Where(match);
            }
            if (include != null)
            {
                query = include(query);
            }
            
            return await query.ToListAsync();
        }

        public void Remove(T t)
        {
            _context.Set<T>().RemoveRange(t);
        }

        public void RemoveAll(ICollection<T> t)
        {
            _context.Set<T>().RemoveRange(t);
        }
    }
}
