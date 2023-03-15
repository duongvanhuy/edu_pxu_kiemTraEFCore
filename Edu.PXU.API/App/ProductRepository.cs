using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Edu.PXU.EntityFECore.Entity;
using System.Linq.Expressions;

namespace Edu.PXU.API.App
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly PXUDBContext _context;

        public ProductRepository(PXUDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
