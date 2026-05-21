using AutoMapper;
using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;

namespace FoodGo.CatalogService.Application.Tests.Features.Restaurants.Commands
{
    public class CreateRestaurantCommandHandlerTests
    {
        private readonly IRestaurantRepository _repository;
        private readonly CreateRestaurantCommandHandler _handler;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRestaurantCommandHandlerTests()
        {
            _repository = Substitute.For<IRestaurantRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new CreateRestaurantCommandHandler(_repository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_WhenRestaurantNameAlreadyExists_ShouldReturnFailure()
        {
            //Arrange
            var command = CreateValidCommand();

            _repository
                .AnyByNameAsync(command.Name)
                .Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();

            _repository.DidNotReceive()
                .Add(Arg.Any<Restaurant>());

            await _unitOfWork.DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ShouldCreateRestaurantAndReturnSuccess()
        {
            // Arrange
            var command = CreateValidCommand();

            _repository
                .AnyByNameAsync(command.Name)
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty();
            result.Value.Name.Should().Be(command.Name);


            await _unitOfWork.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        private static CreateRestaurantCommand CreateValidCommand()
        {
            return new CreateRestaurantCommand

            {
                Name = "Hevi Restaurant",
                Address = new AddressDto
                {
                    Street = "Street",
                    District = "District",
                    City = "City",
                    Latitude = 41,
                    Longitude = 29
                }
            };
        }
    }
}