using AutoMapper;
using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
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

namespace FoodGo.CatalogService.Application.Tests.Features.Restaurants.Commands
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly IRestaurantRepository _repository;
        private readonly UpdateRestaurantCommandHandler _handler;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRestaurantCommandHandlerTests()
        {
            _repository = Substitute.For<IRestaurantRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new UpdateRestaurantCommandHandler(
                _repository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_WhenRestaurantNotFound_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _repository
                .GetByIdAsync(command.Id)
                .Returns((Restaurant?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            await _unitOfWork.DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async Task Handle_WhenRestaurantIsInactive_ShouldReturnFailure()
        {
            var restaurant = CreateRestaurant(active: false);
            var command = CreateValidCommand();

            _repository.GetByIdAsync(command.Id).Returns(restaurant);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            await _unitOfWork.DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenNameChangedAndAlreadyExists_ShouldReturnFailure()
        {
            var restaurant = CreateRestaurant();
            var command = CreateValidCommand(name: "New Name");

            _repository.GetByIdAsync(command.Id).Returns(restaurant);
            _repository.AnyByNameAsync("New Name").Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ShouldUpdateRestaurantAndReturnSuccess()
        {
            var restaurant = CreateRestaurant();
            var command = CreateValidCommand(name: "Updated Name");

            _repository.GetByIdAsync(command.Id).Returns(restaurant);
            _repository.AnyByNameAsync("Updated Name").Returns(false);


            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            restaurant.Name.Should().Be("Updated Name");

            await _unitOfWork.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }


        private static Restaurant CreateRestaurant(bool active = true)
        {
            var restaurant = new Restaurant(
                "Old Name",
                new Address("Street", "District", "City", 41, 29));

            if (!active)
                restaurant.Deactivate();

            return restaurant;
        }

        private static UpdateRestaurantCommand CreateValidCommand(string name = "Old Name")
        {
            return new UpdateRestaurantCommand
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = new AddressDto
                {
                    Street = "New",
                    District = "District",
                    City = "City",
                    Latitude = 40,
                    Longitude = 28
                }
            };
        }

    }
}
