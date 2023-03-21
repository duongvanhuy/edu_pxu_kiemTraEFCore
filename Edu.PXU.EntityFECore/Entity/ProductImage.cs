using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Entity
{
    public  class ProductImage
    {
        public string IdProduct { get; set; }
        public string IdImage { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime?  DateUpdate { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }
        public virtual Image Image { get; set; }
    }
}
