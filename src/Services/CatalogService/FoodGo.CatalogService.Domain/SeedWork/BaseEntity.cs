using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.SeedWork
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; }

        private int? _requestedHashCode;

        //domain içindeki olayları başka katmanlara yaymadan önce burada listeleriz.
        private List<IDomainEvent>? _domainEvents;
        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();
        protected BaseEntity()
        {

        }

        #region Domain Events 

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents?.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent domainItem)
        {
            _domainEvents?.Remove(domainItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        #endregion

        #region Equality (Identity based)

        public bool IsTransient()
        {
            return Id == default;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is BaseEntity other))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (IsTransient() || other.IsTransient())
                return false;

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                _requestedHashCode ??= (Id.GetHashCode() ^ 31);
                return _requestedHashCode.Value;

            }
            return base.GetHashCode();

        }

        public static bool operator ==(BaseEntity? left, BaseEntity? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseEntity? left, BaseEntity? right)
        {
            return !(left == right);
        }

        #endregion
    }

}
