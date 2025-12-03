using FoodGo.CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Infrastructure.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.CategoryId)
                .IsRequired();

            builder.Property(p => p.RestaurantId)
                .IsRequired();

            builder.HasMany(p => p.Prices)
                .WithOne()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Restaurant>()
               .WithMany()
               .HasForeignKey(p => p.RestaurantId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(p => p.Options, option =>
            {
                option.ToTable("ProductOptions");
                option.WithOwner().HasForeignKey("ProductId");

                option.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

                option.OwnsOne(o => o.AdditionalPrice, money =>
                {
                    money.Property(m => m.Amount)
                    .HasPrecision(18, 2)
                    .HasColumnName("AdditionalPriceAmount");

                    money.Property(m => m.Currency)
                    .HasMaxLength(3)
                    .HasColumnName("AdditionalPriceCurrency");
                });
            });
        }
    }
}
