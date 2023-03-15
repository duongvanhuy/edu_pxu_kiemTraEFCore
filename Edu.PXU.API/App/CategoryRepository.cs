using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.App
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly PXUDBContext _context;

        public CategoryRepository(PXUDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
