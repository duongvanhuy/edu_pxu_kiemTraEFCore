using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.PXU.EntityFECore.Data
{
    public class PXUDBContext : IdentityDbContext<UserIdentity, Role, string>
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
                 .HasForeignKey(k => k.IdProduct)
                 .OnDelete(DeleteBehavior.Cascade); // thêm phương thức OnDelete

            modelBuilder.Entity<ProductImage>()
                .HasOne(p => p.Image)
                .WithMany(i => i.ProductImages)
                .HasForeignKey(k => k.IdImage)
                .OnDelete(DeleteBehavior.Cascade); // thêm phương thức OnDelete

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(k => k.IdCategory);

            modelBuilder.Entity<Product>()
               .HasMany(p => p.ProductImages)
               .WithOne(pi => pi.Product)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Image>()
                .HasMany(i => i.ProductImages)
                .WithOne(pi => pi.Image)
                .OnDelete(DeleteBehavior.Cascade); // thêm phương thức OnDelete

            base.OnModelCreating(modelBuilder);
        }
    }
}
