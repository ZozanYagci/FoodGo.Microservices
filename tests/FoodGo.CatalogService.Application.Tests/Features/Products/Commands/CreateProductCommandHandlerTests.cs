using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Products.Commands.CreateProduct;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Tests.Features.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly CreateProductCommandHandler _handler;

        private readonly Guid _restaurantId = Guid.NewGuid();
        private readonly Guid _categoryId = Guid.NewGuid();

        public CreateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _restaurantRepository = Substitute.For<IRestaurantRepository>();
            _categoryRepository = Substitute.For<ICategoryRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new CreateProductCommandHandler(_productRepository, _restaurantRepository, _categoryRepository, _unitOfWork);
        }

        private CreateProductCommand CreateValidCommand()
        {
            return new CreateProductCommand
            {
                Name = "Lahmacun",
                Description = "Yeşillik, limon ile",
                CategoryId = _categoryId,
                RestaurantId = _restaurantId,
                PriceAmount = 350,
                Currency = "TRY"
            };
        }

        [Fact]
        public async Task Handle_WhenRestaurantDoesNotExist_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _restaurantRepository.ExistsAsync(command.RestaurantId, Arg.Any<CancellationToken>())
                .Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            result.Errors.Should().Contain(RestaurantErrors.NotFound(command.RestaurantId));

            await _categoryRepository.DidNotReceive().ExistsAsync(
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>());

            await _productRepository.DidNotReceive().ExistsAsync(
                Arg.Any<Guid>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>());

            _productRepository.DidNotReceive()
                .Add(Arg.Any<Product>());

            await _unitOfWork.DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _restaurantRepository.ExistsAsync(command.RestaurantId, Arg.Any<CancellationToken>())
                .Returns(true);

            _categoryRepository.ExistsAsync(command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            result.Errors.Should().Contain(CategoryErrors.NotFound(command.CategoryId));

            await _productRepository
               .DidNotReceive()
               .ExistsAsync(
                   Arg.Any<Guid>(),
                   Arg.Any<string>(),
                   Arg.Any<CancellationToken>());

            _productRepository
                .DidNotReceive()
                .Add(Arg.Any<Product>());

            await _unitOfWork
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenProductNameAlreadyExists_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _restaurantRepository.ExistsAsync(command.RestaurantId, Arg.Any<CancellationToken>())
                .Returns(true);

            _categoryRepository.ExistsAsync(command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(true);

            _productRepository.ExistsAsync(command.RestaurantId, command.Name, Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            result.Errors.Should().Contain(ProductErrors.NameAlreadyExists(command.Name));

            _productRepository.DidNotReceive().Add(Arg.Any<Product>());

            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldCreateProductSuccessfully()
        {
            var command = CreateValidCommand();

            _restaurantRepository.ExistsAsync(command.RestaurantId, Arg.Any<CancellationToken>())
                .Returns(true);

            _categoryRepository.ExistsAsync(command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(true);

            _productRepository.ExistsAsync(command.RestaurantId, command.Name, Arg.Any<CancellationToken>())
                .Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be(command.Name);
        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldAddProduct()
        {
            var command = CreateValidCommand();

            _restaurantRepository.ExistsAsync(command.RestaurantId, Arg.Any<CancellationToken>())
                .Returns(true);

            _categoryRepository.ExistsAsync(command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(true);

            _productRepository.ExistsAsync(command.RestaurantId, command.Name, Arg.Any<CancellationToken>())
                .Returns(false);

            await _handler.Handle(command, CancellationToken.None);

            _productRepository.Received(1).Add(Arg.Is<Product>(product =>
            product.Name == command.Name &&
            product.Description == command.Description &&
            product.CategoryId == command.CategoryId &&
            product.RestaurantId == command.RestaurantId &&
            product.IsActive &&
            product.Prices.Count == 1 &&
            product.Prices.First().Price.Amount == command.PriceAmount &&
            product.Prices.First().Price.Currency == command.Currency));

        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldSaveChanges()
        {
            var command = CreateValidCommand();

            _restaurantRepository.ExistsAsync(command.RestaurantId, Arg.Any<CancellationToken>())
                .Returns(true);

            _categoryRepository.ExistsAsync(command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(true);

            _productRepository.ExistsAsync(command.RestaurantId, command.Name, Arg.Any<CancellationToken>())
                .Returns(false);

            await _handler.Handle(command, CancellationToken.None);

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());


        }


    }
}
