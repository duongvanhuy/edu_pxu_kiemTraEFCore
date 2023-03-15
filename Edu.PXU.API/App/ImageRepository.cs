using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.App
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly PXUDBContext _context;

        public ImageRepository(PXUDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
