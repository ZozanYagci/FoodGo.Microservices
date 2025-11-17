using FoodGo.CatalogService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.ValueObjects
{
    public sealed class ProductOption : ValueObject
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return AdditionalPrice;
        }

        public override string ToString() => $"{Name} (+{AdditionalPrice})";

    }
}
