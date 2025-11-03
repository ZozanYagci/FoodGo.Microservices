using FoodGo.CatalogService.Domain.Common;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Events.DomainEvents
{
    public record ProductPriceChangedDomainEvent(Guid ProductId, Money NewPrice) : IDomainEvent
    {
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
