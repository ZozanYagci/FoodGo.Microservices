using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        private const int MaxCategoryLimit = 10;

        private Restaurant()
        {

        }

        public Restaurant(string name, Address address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Restaurant.Name.Empty");
            Name = name;

            Address = address ?? throw new DomainException("Restaurant.Address.Null");

            IsActive = true;
            TouchCreated();

        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Restaurant.Name.Empty");

            Name = name;
            TouchUpdated();
        }

        public void SetAddress(Address address)
        {
            Address = address ?? throw new DomainException("Restaurant.Address.Null");
            TouchUpdated();

        }


        public void AddCategory(Guid categoryId)
        {
            if (!IsActive)
                throw new DomainException("Restaurant.Inactive");

            if (_categoryIds.Contains(categoryId))
                throw new DomainException("Restaurant.Category.Duplicate");

            if (_categoryIds.Count >= MaxCategoryLimit)
                throw new DomainException("Restaurant.Category.LimitExceeded");

            _categoryIds.Add(categoryId);
            TouchUpdated();

        }

        public void RemoveCategory(Guid categoryId)
        {
            if (!_categoryIds.Contains(categoryId)) return;

            _categoryIds.Remove(categoryId);
            TouchUpdated();
        }

        public void Activate()
        {
            if (IsActive) return;

            IsActive = true;
            TouchUpdated();
        }

        public void Deactivate()
        {
            if (!IsActive) return;

            IsActive = false;
            TouchUpdated();
        }
    }
}
