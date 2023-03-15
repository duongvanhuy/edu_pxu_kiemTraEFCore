using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.App
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly PXUDBContext _context;

        public ProductImageRepository(PXUDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
