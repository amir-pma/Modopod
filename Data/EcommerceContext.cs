using System;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class EcommerceContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        { }

        public DbSet<Design> Designs { get; set; }
        public DbSet<DesignPosition> DesignPositions { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DesignPosition>(entity =>
            {
                entity.HasOne(dp => dp.Template)
                    .WithMany(p => p.DesignPositions)
                    .HasForeignKey(dp => dp.ProductId);

                entity.HasOne(dp => dp.Design)
                .WithMany(d => d.DesignPositions)
                .HasForeignKey(dp => dp.DesignId);
            });
                                                                          
            modelBuilder.Entity<DesignPosition>()
                .HasKey(dp => new { dp.DesignId, dp.ProductId });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity
                    .HasIndex(b => b.Slug)
                    .IsUnique();
            });

            modelBuilder.Entity<AppUser>()
                .HasIndex(x => x.RefreshToken)
                .IsUnique();
        }
    }
}