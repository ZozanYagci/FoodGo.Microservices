using FoodGo.CatalogService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Events.DomainEvents
{
    public record ProductCreatedDomainEvent(Guid ProductId) : IDomainEvent
    {
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
}
