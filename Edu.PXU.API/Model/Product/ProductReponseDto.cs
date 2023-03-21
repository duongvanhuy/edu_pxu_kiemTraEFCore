using Edu.PXU.API.Model.Images;
using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.Model.Product
{
    public class ProductReponseDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public DateTime? DateUpdate { get; set; } = DateTime.Now;
    //    public List<ImageResponseDto> Images { get; set; }
        public List<string>? FileName { get; set; }
    }
}
