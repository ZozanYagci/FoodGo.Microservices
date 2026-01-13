using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.SeedWork.DomainErrors;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Entities
{
    public class Restaurant : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; } = true;

        public Address Address { get; private set; } //value object

        private readonly List<Guid> _categoryIds = new();
        public IReadOnlyCollection<Guid> CategoryIds => _categoryIds.AsReadOnly();

        private Restaurant()
        {

        }

        public Restaurant(string name, Address address = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(RestaurantErrors.NameCannotBeEmpty);

            Name = name;
            Address = address ?? throw new DomainException(RestaurantErrors.AddressCannotBeNull);
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new DomainException(SeedWork.DomainErrors.RestaurantErrors.NameCannotBeEmpty);

            if (Name == newName)
                return;

            Name = newName;
            TouchUpdated();
        }

        public void UpdateAddress(Address newAddress)
        {
            Address = newAddress ?? throw new DomainException(RestaurantErrors.AddressCannotBeNull);
            TouchUpdated();
        }

        public void AddCategory(Guid categoryId)
        {
            if (!IsActive)
                throw new DomainException(RestaurantErrors.RestaurantInactive);

            if (_categoryIds.Contains(categoryId))
                throw new DomainException(RestaurantErrors.CategoryAlreadyExist);

            _categoryIds.Add(categoryId);
        }

        public void ToggleActive() => IsActive = !IsActive;
    }
}
