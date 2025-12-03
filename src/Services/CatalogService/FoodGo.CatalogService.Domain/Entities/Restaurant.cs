using FoodGo.CatalogService.Domain.SeedWork;
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
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("restaurant adı boş olamaz") : name;
            Address = address ?? throw new DomainException("Adres bilgisi boş olamaz.");
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new DomainException("restaurant adı boş olamaz");
            Name = newName;
            TouchUpdated();
        }

        public void UpdateAddress(Address newAddress)
        {
            Address = newAddress ?? throw new DomainException("Yeni adres boş olamaz");
            TouchUpdated();
        }

        public void AddCategory(Guid categoryId)
        {
            if (!_categoryIds.Contains(categoryId)) _categoryIds.Add(categoryId);
        }

        public void ToggleActive() => IsActive = !IsActive;
    }
}
