using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.Model.Product
{
    public class ProductRequestDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public string IdCategory { get; set; }
        public List<IFormFile>? ImagePro { get; set; }
    }
}
