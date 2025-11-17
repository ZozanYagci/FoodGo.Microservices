using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.SeedWork
{
    // her entity'nin ne zaman oluşturulduğunu ve güncellendiğini kaydeder
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected AuditableEntity()
        {
            TouchCreated();
        }

        protected void TouchCreated() => CreatedAt = DateTime.UtcNow;
        protected void TouchUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}
