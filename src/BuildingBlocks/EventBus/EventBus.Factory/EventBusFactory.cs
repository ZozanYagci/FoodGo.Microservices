using EventBus.Base;
using EventBus.Base.Abstractions;
using EventBus.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Factory
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return new EventBusRabbitMQ(config, serviceProvider);
        }
    }
}
