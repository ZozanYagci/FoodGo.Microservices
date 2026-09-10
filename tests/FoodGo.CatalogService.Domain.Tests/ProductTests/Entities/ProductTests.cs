using FluentAssertions;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.ValueObjects;

namespace FoodGo.CatalogService.Domain.Tests.ProductTests.Entities
{
    public class ProductTests
    {
        private readonly Guid _categoryId = Guid.NewGuid();
        private readonly Guid _restaurantId = Guid.NewGuid();

        private Product CreateValidProduct()
        {
            return new Product(
                "Adana Kebap",
                "Közlenmiş biber, domates ve lavaş ile",
                _categoryId,
                _restaurantId,
                new Money(550, "TRY"));
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateProduct()
        {
            var product = CreateValidProduct();

            product.Name.Should().Be("Adana Kebap");
            product.Description.Should().Be("Közlenmiş biber, domates ve lavaş ile");
            product.CategoryId.Should().Be(_categoryId);
            product.RestaurantId.Should().Be(_restaurantId);
            product.IsActive.Should().BeTrue();

            product.Prices.Should().HaveCount(1);
            product.Prices.First().Price.Should().Be(new Money(550, "TRY"));
            product.CreatedAt.Should().NotBe(default);
            product.UpdatedAt.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowDomainException()
        {
            var act = () => new Product(
                "",
                "",
                _categoryId,
                _restaurantId,
                new Money(550));

            act.Should().Throw<DomainException>()
                .WithMessage("Product.Name.Empty");
        }

        [Fact]
        public void Constructor_WithWhitespaceName_ShouldThrowDomainException()
        {
            var act = () => new Product(
                "   ",
                "Közlenmiş biber, domates ve lavaş ile",
                _categoryId,
                _restaurantId,
                new Money(550));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Name.Empty");
        }

        [Fact]
        public void Constructor_ShouldTrimName()
        {
            var product = new Product(
                "  Adana Kebap  ",
                "Közlenmiş biber, domates ve lavaş ile",
                _categoryId,
                _restaurantId,
                new Money(550));

            product.Name.Should().Be("Adana Kebap");
        }

        [Fact]
        public void Constructor_WithDescriptionLongerThan1000Characters_ShouldThrowDomainException()
        {
            var description = new string('a', 1001);

            var act = () => new Product(
                "Adana Kebap",
                description,
                _categoryId,
                _restaurantId,
                new Money(550));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Description.TooLong");
        }

        [Fact]
        public void Constructor_WithNullDescription_ShouldUseEmptyString()
        {
            var product = new Product(
                "Adana Kebap",
                null!,
                _categoryId,
                _restaurantId,
                new Money(550));

            product.Description.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithEmptyCategoryId_ShouldThrowDomainException()
        {
            var act = () => new Product(
                "Adana Kebap",
                "Közlenmiş biber, domates ve lavaş ile",
                Guid.Empty,
                _restaurantId,
                new Money(550));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Category.Required");
        }

        [Fact]
        public void Constructor_WithEmptyRestaurantId_ShouldThrowDomainException()
        {
            var act = () => new Product(
                "Adana Kebap",
                "Közlenmiş biber, domates ve lavaş ile",
                _categoryId,
                Guid.Empty,
                new Money(250));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Restaurant.Required");
        }

        [Fact]
        public void SetName_WithValidDifferentName_ShouldUpdateName()
        {
            var product = CreateValidProduct();

            product.SetName("Urfa Kebap");
            product.Name.Should().Be("Urfa Kebap");
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void SetName_WithSameName_ShouldNotUpdateProduct()
        {
            var product = CreateValidProduct();

            product.SetName("Adana Kebap");

            product.Name.Should().Be("Adana Kebap");
            product.UpdatedAt.Should().BeNull();
        }

        [Fact]
        public void SetName_WithEmptyName_ShouldThrowDomainException()
        {
            var product = CreateValidProduct();

            var act = () => product.SetName("");

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Name.Empty");


        }

        [Fact]
        public void UpdateDescription_WithValidDifferentDescription_ShouldUpdateDescription()
        {
            var product = CreateValidProduct();

            product.UpdateDescription("Közlenmiş biber, domates, lavaş ve salata ile");

            product.Description.Should().Be("Közlenmiş biber, domates, lavaş ve salata ile");
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void UpdateDescription_WithSameDescription_ShouldNotUpdateProduct()
        {
            var product = CreateValidProduct();

            product.UpdateDescription("Közlenmiş biber, domates ve lavaş ile");

            product.Description.Should().Be("Közlenmiş biber, domates ve lavaş ile");
            product.UpdatedAt.Should().BeNull();
        }

        [Fact]

        public void ChangePrice_WithDifferentPrice_ShouldAddNewPrice()
        {
            var product = CreateValidProduct();

            product.ChangePrice(new Money(600, "TRY"));

            product.Prices.Should().HaveCount(2);
            product.Prices.Last().Price.Should().Be(new Money(600, "TRY"));
            product.Prices.First().To.Should().NotBeNull();
            product.Prices.Last().To.Should().BeNull();
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void ChangePrice_WithSamePrice_ShouldNotAddNewPrice()
        {
            var product = CreateValidProduct();

            product.ChangePrice(new Money(550, "TRY"));

            product.Prices.Should().HaveCount(1);
            product.UpdatedAt.Should().BeNull();
        }

        [Fact]
        public void AddImage_WithValidUrl_ShouldAddImage()
        {
            var product = CreateValidProduct();

            product.AddImage("https://example.com/adana-kebap.jpg");

            product.Images.Should().HaveCount(1);
            product.Images.First().Url.Should().Be("https://example.com/adana-kebap.jpg");
            product.Images.First().IsPrimary.Should().BeTrue();
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void AddImage_WithPrimaryImage_ShouldMakeItPrimary()
        {
            var product = CreateValidProduct();

            product.AddImage("image-1.jpg");
            product.AddImage("image-2.jpg", isPrimary: true);

            product.Images.Should().HaveCount(2);
            product.Images.Single(x => x.Url == "image-2.jpg").IsPrimary.Should().BeTrue();
            product.Images.Single(x => x.Url == "image-1.jpg").IsPrimary.Should().BeFalse();
        }

        [Fact]
        public void AddImage_WithDuplicateUrl_ShouldThrowDomainException()
        {
            var product = CreateValidProduct();

            product.AddImage("urfa-kebap.jpg");

            var act = () => product.AddImage("urfa-kebap.jpg");

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Image.Duplicate");
        }

        [Fact]
        public void AddImage_WithEmptyUrl_ShouldThrowDomainException()
        {
            var product = CreateValidProduct();

            var act = () => product.AddImage("");

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Image.Empty");
        }

        [Fact]
        public void AddOption_WithValidOption_ShouldAddOption()
        {
            var product = CreateValidProduct();

            var option = new ProductOption(
                "Acılı Ezme",
                new Money(60, "TRY"));

            product.AddOption(option);

            product.Options.Should().ContainSingle();
            product.Options.First().Should().Be(option);
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void AddOption_WithDuplicateOption_ShouldThrowDomainException()
        {
            var product = CreateValidProduct();

            var option = new ProductOption(
                "Acılı Ezme",
                new Money(60, "TRY"));

            product.AddOption(option);

            var act = () => product.AddOption(option);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Product.Option.Duplicate");
        }

        [Fact]
        public void Deactivate_WhenProductIsActive_ShouldDeactivateProduct()
        {
            var product = CreateValidProduct();

            product.Deactivate();

            product.IsActive.Should().BeFalse();
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void Deactivate_WhenProductIsAlreadyInactive_ShouldDoNothing()
        {
            var product = CreateValidProduct();

            product.Deactivate();
            var updatedAt = product.UpdatedAt;

            product.Deactivate();

            product.IsActive.Should().BeFalse();
            product.UpdatedAt.Should().Be(updatedAt);
        }

        [Fact]
        public void Activate_WhenProductIsInactive_ShouldActivateProduct()
        {
            var product = CreateValidProduct();

            product.Deactivate();
            product.Activate();

            product.IsActive.Should().BeTrue();
            product.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void Activate_WhenProductIsAlreadyActive_ShouldDoNothing()
        {
            var product = CreateValidProduct();

            product.Activate();

            product.IsActive.Should().BeTrue();
            product.UpdatedAt.Should().BeNull();
        }
    }

}