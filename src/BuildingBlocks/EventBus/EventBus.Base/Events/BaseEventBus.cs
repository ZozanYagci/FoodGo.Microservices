using EventBus.Base.Abstractions;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus, IDisposable
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IEventBusSubscriptionManager subsManager;
        protected readonly EventBusConfig _config;

        protected BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            subsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        protected virtual string ProcessEventName(string eventName)
        {
            if (_config.DeleteEventPrefix && !string.IsNullOrEmpty(_config.EventNamePrefix) &&
                eventName.StartsWith(_config.EventNamePrefix))
            {
                eventName = eventName.Substring(_config.EventNamePrefix.Length);
            }

            if (_config.DeleteEventSuffix && !string.IsNullOrEmpty(_config.EventNameSuffix) &&
                eventName.EndsWith(_config.EventNameSuffix))

            {
                eventName = eventName.Substring(0, eventName.Length - _config.EventNameSuffix.Length);
            }
            return eventName;
        }
        protected virtual string GetSubName(string eventName)
        => $"{_config.SubscriberClientAppName}.{ProcessEventName(eventName)}";

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            eventName = ProcessEventName(eventName);
            if (!subsManager.HasSubscriptionsForEvent(eventName))
                return false;

            var subscriptions = subsManager.GetHandlersForEvent(eventName);

            using var scope = _serviceProvider.CreateScope();

            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                if (handler == null) continue;

                var eventType = subsManager.GetEventTypeByName(eventName);
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                var method = concreteType.GetMethod("Handle");

                await (Task)method.Invoke(handler, new[] { integrationEvent });
            }

            return true;
        }

        public virtual void Dispose()
        {

        }

        public abstract Task Publish(IntegrationEvent @event);


        public abstract void Subscribe<T, TH>(string? queueName = null)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;


        public abstract void UnSubscribe<T, TH>(string? queueName = null)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

    }
}
