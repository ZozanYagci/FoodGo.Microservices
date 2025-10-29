using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Abstractions
{
    public interface IEventBus
    {
        Task Publish(IntegrationEvent @event);

        // T --> hangi event tipi (örneğin ProductCreatedIntegrationEvent)
        // TH --> o event'i dinleyen handler tipi (ProductCreatedIntegrationEventHandler)
        void Subscribe<T, TH>(string? queueName = null)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void UnSubscribe<T, TH>(string? queueName = null)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
