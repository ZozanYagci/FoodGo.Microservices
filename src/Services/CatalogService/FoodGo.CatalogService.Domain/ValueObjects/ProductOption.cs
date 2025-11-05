using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.ValueObjects
{
    public sealed class ProductOption : IEquatable<ProductOption>
    {
        public string Name { get; }
        public Money AdditionalPrice { get; }

        private ProductOption()
        {

        }
        public ProductOption(string name, Money additionalPrice)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Ürün seçimi boş geçilemez") : name;
            AdditionalPrice = additionalPrice ?? throw new ArgumentNullException(nameof(additionalPrice));

        }
        public bool Equals(ProductOption? other) => other is not null && Name == other.Name && AdditionalPrice.Equals(other.AdditionalPrice);
        public override int GetHashCode() => HashCode.Combine(Name, AdditionalPrice);

    }
}
