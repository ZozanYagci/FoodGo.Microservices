using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Products.Queries.GetProductById;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Tests.Features.Products.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly IProductRepository _productRepository;

        private readonly GetProductByIdQueryHandler _handler;

        private readonly Guid _productId = Guid.NewGuid();
        private readonly Guid _restaurantId = Guid.NewGuid();
        private readonly Guid _categoryId = Guid.NewGuid();

        public GetProductByIdQueryHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();

            _handler = new GetProductByIdQueryHandler(
                _productRepository);
        }

        private Product CreateProduct()
        {
            var product = new Product(
                "Adana Kebap",
                "Közlenmiş biber ve soğan ile servis edilir.",
                _categoryId,
                _restaurantId,
                new Money(450, "TRY"));

            product.AddImage(
                 "https://example.com/adana.jpg",
                  isPrimary: true);

            product.AddOption(
                new ProductOption(
                    "Acı Sos",
                    new Money(20, "TRY")));

            return product;
        }

        [Fact]
        public async Task Handle_WhenProductDoesNotExist_ShouldReturnFailure()
        {
            var query = new GetProductByIdQuery
            {
                Id = _productId
            };

            _productRepository
                .GetByIdAsync(
                    query.Id,
                    Arg.Any<CancellationToken>())
                .Returns((Product?)null);


            var result = await _handler.Handle(
                query,
                CancellationToken.None);


            result.IsFailure.Should().BeTrue();

            result.Errors.Should()
                .Contain(ProductErrors.NotFound(query.Id));
        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldReturnSuccess()
        {

            var product = CreateProduct();

            var query = new GetProductByIdQuery
            {
                Id = product.Id
            };

            _productRepository
                .GetByIdAsync(
                    query.Id,
                    Arg.Any<CancellationToken>())
                .Returns(product);


            var result = await _handler.Handle(
                query,
                CancellationToken.None);


            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldMapBasicProductProperties()
        {
            var product = CreateProduct();

            var query = new GetProductByIdQuery
            {
                Id = product.Id
            };

            _productRepository.GetByIdAsync(
                query.Id,
                Arg.Any<CancellationToken>())
                .Returns(product);

            var result = await _handler.Handle(
                query, CancellationToken.None);

            var response = result.Value;

            response.Id.Should().Be(product.Id);
            response.Name.Should().Be(product.Name);
            response.Description.Should().Be(product.Description);
            response.CategoryId.Should().Be(product.CategoryId);
            response.RestaurantId.Should().Be(product.RestaurantId);
            response.IsActive.Should().Be(product.IsActive);
        }

        [Fact]

        public async Task Handle_WhenProductExists_ShouldMapPrices()
        {
            var product = CreateProduct();

            var query = new GetProductByIdQuery
            {
                Id = product.Id
            };

            _productRepository
               .GetByIdAsync(
                   query.Id,
                   Arg.Any<CancellationToken>())
               .Returns(product);


            var result = await _handler.Handle(
                query,
                CancellationToken.None);

            var price = result.Value.Prices.Should()
              .ContainSingle()
              .Subject;

            price.Amount.Should().Be(450);
            price.Currency.Should().Be("TRY");
        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldMapImages()
        {

            var product = CreateProduct();

            var query = new GetProductByIdQuery
            {
                Id = product.Id
            };

            _productRepository
                .GetByIdAsync(
                    query.Id,
                    Arg.Any<CancellationToken>())
                .Returns(product);


            var result = await _handler.Handle(
                query,
                CancellationToken.None);


            var image = result.Value.Images.Should()
                .ContainSingle()
                .Subject;

            image.Url.Should().Be("https://example.com/adana.jpg");
            image.IsPrimary.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldMapOptions()
        {

            var product = CreateProduct();

            var query = new GetProductByIdQuery
            {
                Id = product.Id
            };

            _productRepository
                .GetByIdAsync(
                    query.Id,
                    Arg.Any<CancellationToken>())
                .Returns(product);


            var result = await _handler.Handle(
                query,
                CancellationToken.None);


            var option = result.Value.Options.Should()
                .ContainSingle()
                .Subject;

            option.Name.Should().Be("Acı Sos");
            option.AdditionalPrice.Should().Be(20);
            option.Currency.Should().Be("TRY");
        }

    }
}

