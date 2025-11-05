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
    public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.ToTable("ProductPrices");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.From)
                .IsRequired();

            builder.Property(p => p.To);

            builder.Property<Guid>("ProductId");

            builder.OwnsOne(p => p.Price, money =>
            {
                money.Property(m => m.Amount)
                .HasColumnName("PriceAmount")
                .IsRequired();

                money.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .IsRequired();
            });

        }
    }
}
