using AutoMapper.Configuration.Annotations;
using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Products.Commands.DeleteProduct;
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
    public class DeleteProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new DeleteProductCommandHandler(
                _productRepository, _unitOfWork);
        }

        private Product CreateProduct()
        {
            return new Product(
                "Adana Kebap",
                "Közlenmiş biber ve lavaş ile",
                Guid.NewGuid(),
                Guid.NewGuid(),
                new Money(450, "TRY"));
        }

        [Fact]
        public async Task Handle_WhenProductDoesNotExist_ShouldReturnFailure()
        {
            var productId = Guid.NewGuid();

            var command = new DeleteProductCommand
            {
                Id = productId,
            };

            _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            var result = await _handler.Handle(
                command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            result.Errors.Should()
                .Contain(ProductErrors.NotFound(productId));

            _productRepository.DidNotReceive()
                .Add(Arg.Any<Product>());

            await _unitOfWork
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldMarkProductAsDeleted()
        {
            var product = CreateProduct();

            var command = new DeleteProductCommand
            {
                Id = product.Id
            };

            _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
                .Returns(product);

            await _handler.Handle(
                command, CancellationToken.None);

            product.IsDeleted.Should().BeTrue();
            product.DeletedAt.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldSaveChanges()
        {
            var product = CreateProduct();

            var command = new DeleteProductCommand
            {
                Id = product.Id
            };

            _productRepository
               .GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
               .Returns(product);


            await _handler.Handle(
                command,
                CancellationToken.None);

            await _unitOfWork.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenProductExists_ShouldReturnSuccess()
        {
            var product = CreateProduct();

            var command = new DeleteProductCommand
            {
                Id = product.Id
            };

            _productRepository
              .GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
              .Returns(product);


            var result = await _handler.Handle(
                command,
                CancellationToken.None);


            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(product.Id);
        }
    }
}
