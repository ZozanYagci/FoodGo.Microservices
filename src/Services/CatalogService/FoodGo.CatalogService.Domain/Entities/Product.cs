using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.Events.DomainEvents;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            SetDescription(description);
            SetCategory(categoryId);
            SetRestaurant(restaurantId);

            IsActive = true;

            _prices.Add(new ProductPrice(initialPrice, DateTime.UtcNow));


            // Domain event: product created
            //AddDomainEvent(new ProductCreatedDomainEvent(this.Id));
            TouchCreated();

        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product.Name.Empty");

            Name = name.Trim();
            if (!IsTransient())
                TouchUpdated();
        }

        //public void UpdateName(string newName)
        //{
        //    if (string.IsNullOrWhiteSpace(newName))
        //        throw new DomainException("Ürün adı boş olamaz.");

        //    Name = newName;
        //    TouchUpdated();
        //}

        private void SetDescription(string description)
        {
            Description = description?.Trim() ?? string.Empty;

            if (Description.Length > 1000)
                throw new DomainException("Product.Description.TooLong");

            if (!IsTransient())
                TouchUpdated();
        }

        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        private void SetCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new DomainException("Product.Category.Required");

            CategoryId = categoryId;
        }

        private void SetRestaurant(Guid restaurantId)
        {
            if (restaurantId == Guid.Empty)
                throw new DomainException("Product.Restaurant.Required");

            RestaurantId = restaurantId;
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
                throw new DomainException("Product.Image.Empty");

            if (_images.Any(x => x.Url == url))
                throw new DomainException("Product.Image.Duplicate");

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
                throw new DomainException("Product.Option.Duplicate");

            _options.Add(option);
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
