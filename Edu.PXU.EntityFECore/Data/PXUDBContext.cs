using Edu.PXU.EntityFECore.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Data
{
    public class PXUDBContext : DbContext
    {

        public PXUDBContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImage>()
                 .HasKey(pi => new { pi.IdProduct, pi.IdImage });

            modelBuilder.Entity<ProductImage>()
                 .HasOne(p => p.Product)
                 .WithMany(i => i.ProductImages)
                 .HasForeignKey(k => k.IdProduct);
            
            modelBuilder.Entity<ProductImage>()
                .HasOne(p => p.Image)
                .WithMany(i => i.ProductImages)
                .HasForeignKey(k => k.IdImage);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(k => k.IdCategory);
        }
    }
}
