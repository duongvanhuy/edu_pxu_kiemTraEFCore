using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdate{ get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
