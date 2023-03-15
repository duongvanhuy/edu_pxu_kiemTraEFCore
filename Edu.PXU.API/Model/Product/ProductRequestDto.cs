using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.Model.Product
{
    public class ProductRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public DateTime? DateUpdate { get; set; } = DateTime.Now;
        public int IdCategory { get; set; }
    }
}
