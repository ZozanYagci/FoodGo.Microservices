using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Entities
{
    public class ProductPrice : BaseEntity
    {
        public Money Price { get; private set; }
        public DateTime From { get; private set; }
        public DateTime? To { get; private set; }

        private ProductPrice()
        {

        }

        public ProductPrice(Money price, DateTime from)
        {
            Price = price ?? throw new ArgumentNullException(nameof(price));
            From = from;
        }

        public void Close(DateTime endAt)
        {
            if (endAt <= From) throw new DomainException("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");
            To = endAt;
        }
    }
}
