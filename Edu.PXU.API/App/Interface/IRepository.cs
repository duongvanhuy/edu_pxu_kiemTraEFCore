using System.Linq.Expressions;

namespace Edu.PXU.API.App.Interface
{
    public interface IRepository<T> where T : class
    {
        T Create(T t);
        ICollection<T> GetAll();
        T? Update(T t, object key);
        void Delete(T entity);
        T? Get(int id);
        ICollection<T> Get(Expression<Func<T, bool>> match);
        ICollection<T> Get(Expression<Func<T, bool>> match, int pageSize, int pageIndex, out int total);
    }
}
