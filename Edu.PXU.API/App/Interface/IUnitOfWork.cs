using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore;

namespace Edu.PXU.API.App.Interface
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        IImageRepository ImageRepository { get; }
        IUserIdentityRepository UserIdentityRepository { get; }


        void SaveChanges();
        Task SaveChangesAsync();
        void CreateTransaction();
        void Commit();
        void Rollback();
    }
}
