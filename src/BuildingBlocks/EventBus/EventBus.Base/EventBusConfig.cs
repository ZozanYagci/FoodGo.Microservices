using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
    public class EventBusConfig
    {
        public int ConnectionRetryCount { get; init; } = 5;
        public string DefaultTopicName { get; init; } = "FoodGoEventBus";
        public string EventBusConnectionString { get; init; } = string.Empty;
        public string SubscriberClientAppName { get; init; } = string.Empty;
        public string EventNamePrefix { get; init; } = string.Empty;
        public string EventNameSuffix { get; init; } = "IntegrationEvent";
        public EventBusType EventBusType { get; init; } = EventBusType.RabbitMQ;

        public EventBusConnection? Connection { get; set; }
        public bool DeleteEventPrefix => !string.IsNullOrWhiteSpace(EventNamePrefix);
        public bool DeleteEventSuffix => !string.IsNullOrWhiteSpace(EventNameSuffix);
    }
    public enum EventBusType
    {
        RabbitMQ = 0,
        AzureServiceBus = 1
    }
}

