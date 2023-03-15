using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdate{ get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public int IdCategory { get; set; }
        public virtual Category Category { get; set; }
    }
}
