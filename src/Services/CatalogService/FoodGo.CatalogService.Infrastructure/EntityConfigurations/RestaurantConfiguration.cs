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
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.OwnsOne(r => r.Address, address =>
            {
                address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Street");

                address.Property(a => a.District)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("District");

                address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("City");

                address.Property(a => a.Latitude)
                .IsRequired()
                .HasColumnName("Latitude");

                address.Property(a => a.Longitude)
                .IsRequired()
                .HasColumnName("Longitude");
            });

            builder.Navigation(r => r.Address).IsRequired();

        }
    }
}
