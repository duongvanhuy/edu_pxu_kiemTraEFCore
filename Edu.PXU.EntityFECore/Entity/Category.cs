using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdate{ get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
