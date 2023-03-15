using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Edu.PXU.API.App
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly PXUDBContext _context;
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IProductImageRepository ProductImageRepository { get; }
        public IImageRepository ImageRepository { get; }

      //  private DbContextTransaction _transaction;

        public UnitOfWork(PXUDBContext context ,IProductRepository productRepository, ICategoryRepository categoryRepository, IProductImageRepository productImageRepository, IImageRepository imageRepository)
        {
            _context = context;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            ProductImageRepository = productImageRepository;
            ImageRepository = imageRepository;
        }
        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            //    _transaction.Commit();
            }
            catch (Exception)
            {
            //    _transaction.Rollback();
                throw;
            }
        }

        public void CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
