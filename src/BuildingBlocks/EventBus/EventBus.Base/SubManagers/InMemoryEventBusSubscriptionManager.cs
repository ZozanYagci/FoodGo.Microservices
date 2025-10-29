using EventBus.Base.Abstractions;
using EventBus.Base.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.SubManagers
{
    public sealed class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {

        private readonly ConcurrentDictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly object _lock = new();

        private readonly Func<string, string> _eventNameGetter;

        public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
        {
            _handlers = new();
            _eventTypes = new();
            _eventNameGetter = eventNameGetter;
        }

        public bool IsEmpty => !_handlers.Any();

        public event EventHandler<string> OnEventRemoved;

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            lock (_lock)
            {
                if (!_handlers.ContainsKey(eventName))
                    _handlers[eventName] = new List<SubscriptionInfo>();

                if (_handlers[eventName].Any(s => s.HandlerType == typeof(TH)))
                    throw new ArgumentException(
                        $"Handler {typeof(TH).Name} already registered for '{eventName}'");

                _handlers[eventName].Add(SubscriptionInfo.Typed(typeof(TH)));

                if (!_eventTypes.Contains(typeof(T)))
                    _eventTypes.Add(typeof(T));
            }
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            lock (_lock)
            {
                var subsToRemove =
                    _handlers[eventName].SingleOrDefault(s => s.HandlerType == typeof(TH));

                if (subsToRemove == null)
                    return;

                _handlers[eventName].Remove(subsToRemove);

                if (!_handlers[eventName].Any())
                {
                    _handlers.TryRemove(eventName, out _);

                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    if (eventType != null)
                        _eventTypes.Remove(eventType);

                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
            => _handlers.ContainsKey(eventName)
                ? _handlers[eventName]
                : Enumerable.Empty<SubscriptionInfo>();


        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
            => GetHandlersForEvent(GetEventKey<T>());


        public Type GetEventTypeByName(string eventName)
            => _eventTypes.SingleOrDefault(t => t.Name == eventName);


        public string GetEventKey<T>() => _eventNameGetter(typeof(T).Name);

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
            => HasSubscriptionsForEvent(GetEventKey<T>());

        public bool HasSubscriptionsForEvent(string eventName)
            => _handlers.ContainsKey(eventName);

        public void Clear()
        {
            _handlers.Clear();
            _eventTypes.Clear();
        }

        private void RaiseOnEventRemoved(string eventName)
            => OnEventRemoved?.Invoke(this, eventName);
    }
}
