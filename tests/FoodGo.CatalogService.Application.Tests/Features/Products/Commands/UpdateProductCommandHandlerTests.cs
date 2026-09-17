using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Products.Commands.UpdateProduct;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Tests.Features.Products.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UpdateProductCommandHandler _handler;

        private readonly Guid _restaurantId = Guid.NewGuid();
        private readonly Guid _categoryId = Guid.NewGuid();

        public UpdateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new UpdateProductCommandHandler(_productRepository, _unitOfWork);
        }

        private UpdateProductCommand CreateValidCommand()
        {
            return new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Lahmacun",
                Description = "Yeşillik, limon ile"
            };

        }

        private Product CreateProduct(Guid id)
        {
            var product = new TestProduct(
                "Adana Kebap",
                "Acılı Adana",
                _categoryId,
                _restaurantId,
                new Money(400, "TRY"));

            product.SetId(id);
            return product;
        }


        [Fact]
        public async Task Handle_WhenProductDoesNotExist_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _productRepository.GetByIdAsync(
                command.Id,
                Arg.Any<CancellationToken>()).Returns((Product?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            result.Errors.Should().Contain(
                ProductErrors.NotFound(command.Id));

            await _productRepository.DidNotReceive().ExistsAsync(
                Arg.Any<Guid>(),
                Arg.Any<string>(),
                Arg.Any<Guid?>(),
                Arg.Any<CancellationToken>());

            await _unitOfWork.DidNotReceive()
                .SaveChangesAsync(
                Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task Handle_WhenProductNameAlreadyExists_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            var product = CreateProduct(command.Id);

            _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.ExistsAsync(
                product.RestaurantId,
                command.Name,
                product.Id,
                Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            result.Errors.Should().Contain(
                ProductErrors.NameAlreadyExists(command.Name));

            product.Name.Should().Be("Adana Kebap");
            product.Description.Should().Be("Acılı Adana");

            await _unitOfWork
                .DidNotReceive()
                .SaveChangesAsync(
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldUpdateProductSuccessfully()
        {
            var command = CreateValidCommand();

            var product = CreateProduct(command.Id);

            _productRepository.GetByIdAsync(
                command.Id,
                Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository.ExistsAsync(
                product.RestaurantId,
                command.Name,
                product.Id,
                Arg.Any<CancellationToken>())
                .Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(product.Id);
            result.Value.Name.Should().Be(command.Name);

            product.Name.Should().Be(command.Name);
            product.Description.Should().Be(command.Description);
        }


        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldCheckDuplicateNameExcludingCurrentProduct()
        {
            var command = CreateValidCommand();

            var product = CreateProduct(command.Id);

            _productRepository
               .GetByIdAsync(
                   command.Id,
                   Arg.Any<CancellationToken>())
               .Returns(product);

            _productRepository
                .ExistsAsync(
                    product.RestaurantId,
                    command.Name,
                    product.Id,
                    Arg.Any<CancellationToken>())
                .Returns(false);

            await _handler.Handle(
               command,
               CancellationToken.None);


            await _productRepository
                .Received(1)
                .ExistsAsync(
                    product.RestaurantId,
                    command.Name,
                    product.Id,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldSaveChanges()
        {

            var command = CreateValidCommand();

            var product = CreateProduct(command.Id);

            _productRepository
                .GetByIdAsync(
                    command.Id,
                    Arg.Any<CancellationToken>())
                .Returns(product);

            _productRepository
                .ExistsAsync(
                    product.RestaurantId,
                    command.Name,
                    product.Id,
                    Arg.Any<CancellationToken>())
                .Returns(false);


            await _handler.Handle(
                command,
                CancellationToken.None);


            await _unitOfWork
                .Received(1)
                .SaveChangesAsync(
                    Arg.Any<CancellationToken>());

        }
        private class TestProduct : Product
        {
            public TestProduct(
                string name,
                string description,
                Guid categoryId,
                Guid restaurantId,
                Money initialPrice
                ) : base(name, description, categoryId, restaurantId, initialPrice)
            {

            }

            public void SetId(Guid id)
            {
                Id = id;
            }
        }
    }
}
