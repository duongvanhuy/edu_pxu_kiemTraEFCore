using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore;
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
        
        public T Create(T t)
        {
            _context.Set<T>();
            _context.SaveChanges();
            return t;
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();

        }

        public T? Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public ICollection<T> Get(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public ICollection<T> Get(Expression<Func<T, bool>> match, int pageSize, int pageIndex, out int total)
        {
            // Lấy tất cả các phần tử phù hợp với điều kiện lọc
            var filteredElements = _context.Set<T>().Where(match.Compile());

            // Đếm tổng số phần tử trong bộ sưu tập
            total = filteredElements.Count();

            // Lấy các phần tử cho trang hiện tại bằng cách sử dụng phân trang
            var pageElements = filteredElements.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            // Trả về các phần tử của trang hiện tại dưới dạng ICollection<T>
            return pageElements.ToList();
        }

        public T? Update(T t, object key)
        {
            T? exist = _context.Set<T>().Find(key);

            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                _context.SaveChanges();
            }

            return exist;
        }
        
        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

    }
}
