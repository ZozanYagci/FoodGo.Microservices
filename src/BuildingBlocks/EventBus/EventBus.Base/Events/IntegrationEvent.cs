using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Events
{
    public abstract class IntegrationEvent
    {
        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }
        public int Version { get; init; } = 1; //event versioning support

        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected IntegrationEvent(Guid id, DateTime creationDate, int version)
        {
            Id = id;
            CreationDate = creationDate;
            Version = version;
        }

        public override string ToString()
      => $"{GetType().Name} [Id={Id}, Created={CreationDate}, Version={Version}]";

    }
}
