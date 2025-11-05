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
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.IsPrimary)
                .IsRequired();

            builder.Property<Guid>("ProductId");
        }
    }
}
