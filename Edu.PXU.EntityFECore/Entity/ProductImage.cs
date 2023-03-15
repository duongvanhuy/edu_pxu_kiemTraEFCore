using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Entity
{
    public  class ProductImage
    {
        public int IdProduct { get; set; }
        public int IdImage { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdate { get; set; }

        public virtual Product Product { get; set; }
        public virtual Image Image { get; set; }
    }
}
