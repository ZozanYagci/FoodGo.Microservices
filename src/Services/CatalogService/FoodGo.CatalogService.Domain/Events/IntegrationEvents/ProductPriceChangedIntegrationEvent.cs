using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Events.IntegrationEvents
{
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; set; }
        public decimal NewPrice { get; set; }
        public string Currency { get; set; }

        public ProductPriceChangedIntegrationEvent(Guid productId, decimal newPrice, string currency) : base()
        {
            ProductId = productId;
            NewPrice = newPrice;
            Currency = currency;
        }
    }
}
