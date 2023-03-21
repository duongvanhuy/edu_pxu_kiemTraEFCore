using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Edu.PXU.API.App
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly PXUDBContext _context;
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IProductImageRepository ProductImageRepository { get; }
        public IImageRepository ImageRepository { get; }
        public IUserIdentityRepository UserIdentityRepository { get; }

        private IDbContextTransaction _transaction;

        public UnitOfWork(PXUDBContext context ,IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IProductImageRepository productImageRepository,
            IImageRepository imageRepository,
            IUserIdentityRepository userIdentityRepository)
        {
            _context = context;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            ProductImageRepository = productImageRepository;
            ImageRepository = imageRepository;
            UserIdentityRepository = userIdentityRepository;
        }
        public void Commit()
        {
            try
            {
                _context.SaveChanges();
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction = null;
                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
        }

        public void CreateTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
