using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.Events.DomainEvents;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace FoodGo.CatalogService.Domain.Entities
{
    public class Product : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid RestaurantId { get; private set; }
        public bool IsActive { get; private set; }


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
            SetName(name);
            Description = description ?? string.Empty;
            CategoryId = categoryId;
            RestaurantId = restaurantId;
            IsActive = true;

            var price = new ProductPrice(initialPrice, DateTime.UtcNow);
            _prices.Add(price);

            // Domain event: product created
            //AddDomainEvent(new ProductCreatedDomainEvent(this.Id));
            //TouchCreated();

        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty");

            Name = name;
            TouchUpdated();
        }

        //public void UpdateName(string newName)
        //{
        //    if (string.IsNullOrWhiteSpace(newName))
        //        throw new DomainException("Ürün adı boş olamaz.");

        //    Name = newName;
        //    TouchUpdated();
        //}

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? string.Empty;
            TouchUpdated();
        }

        public void ChangePrice(Money newPrice)
        {
            var current = _prices.LastOrDefault();
            if (current?.Price.Equals(newPrice) == true) return;

            if (current != null) current.Close(DateTime.UtcNow);

            var newPriceEntity = new ProductPrice(newPrice, DateTime.UtcNow);
            _prices.Add(newPriceEntity);
            TouchUpdated();

            //raise domain event
            //AddDomainEvent(new ProductPriceChangedDomainEvent(this.Id, newPrice));
        }

        public void AddImage(string url, bool isPrimary = false)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new DomainException("Image url cannot be empty");

            if (_images.Any(x => x.Url == url))
                throw new DomainException("Image already exists");

            if (isPrimary)
                foreach (var img in _images) img.UnmarkPrimary();

            _images.Add(new ProductImage(url, isPrimary));

            //ensure at least one primary
            if (!_images.Any(x => x.IsPrimary))
                _images.Last().MarkPrimary();

            TouchUpdated();
        }

        public void AddOption(ProductOption option)
        {
            if (_options.Any(o => o.Equals(option)))
                throw new DomainException("Option already exists");

            _options.Add(option);
            TouchUpdated();
        }

        public void Active()
        {
            IsActive = true;
            TouchUpdated();

        }

        public void Deactivate()
        {
            IsActive = false;
            TouchUpdated();
        }
    }
}
