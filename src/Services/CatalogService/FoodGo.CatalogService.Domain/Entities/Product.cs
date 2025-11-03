using FoodGo.CatalogService.Domain.Common;
using FoodGo.CatalogService.Domain.Events.DomainEvents;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid RestaurantId { get; private set; }

        private readonly List<ProductPrice> _prices = new();
        private readonly List<ProductImage> _images = new();
        private readonly List<ProductOption> _options = new();

        public IReadOnlyCollection<ProductPrice> Prices => _prices.AsReadOnly();
        public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
        public IReadOnlyCollection<ProductOption> Options => _options.AsReadOnly();


        private Product()
        {

        }

        public Product(string name, string description, Guid categoryId, Guid restaurantId, Money initialPrice)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Ürün adı boş olamaz") : name;
            Description = description ?? string.Empty;
            CategoryId = categoryId;
            RestaurantId = restaurantId;

            var price = new ProductPrice(initialPrice, DateTime.UtcNow);
            _prices.Add(price);

            // Domain event: product created
            DomainEvents.Add(new ProductCreatedDomainEvent(this.Id));
            TouchCreated();

        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Ürün adı boş olamaz.");

            Name = newName;
            TouchUpdated();
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? string.Empty;
            TouchUpdated();
        }

        public void ChangePrice(Money newPrice)
        {
            var current = _prices.OrderByDescending(p => p.From).FirstOrDefault();
            if (current != null && current.Price.Equals(newPrice)) return;

            if (current != null) current.Close(DateTime.UtcNow);

            var newPriceEntity = new ProductPrice(newPrice, DateTime.UtcNow);
            _prices.Add(newPriceEntity);
            TouchUpdated();

            //raise domain event
            DomainEvents.Add(new ProductPriceChangedDomainEvent(this.Id, newPrice));
        }

        public void AddImage(string url, bool isPrimary = false)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new DomainException("Resim url boş olamaz");

            if (isPrimary)
                foreach (var img in _images) img.UnmarkPrimary();

            _images.Add(new ProductImage(url, isPrimary));
            TouchUpdated();
        }


        // domain events helper (simple list)
        public List<IDomainEvent> DomainEvents { get; } = new();
    }
}
