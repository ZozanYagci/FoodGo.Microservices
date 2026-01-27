using AutoMapper;
using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
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
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly RestaurantBusinessRules _businessRules;
        private readonly UpdateRestaurantCommandHandler _handler;

        public UpdateRestaurantCommandHandlerTests()
        {
            _restaurantRepository = Substitute.For<IRestaurantRepository>();
            _mapper = Substitute.For<IMapper>();
            _businessRules = new RestaurantBusinessRules(_restaurantRepository);

            _handler = new UpdateRestaurantCommandHandler(
                _restaurantRepository, _mapper, _businessRules);
        }

        [Fact]
        public async Task Handle_WhenRestaurantNotFound_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _restaurantRepository
                .GetByIdAsync(command.Request.Id)
                .Returns((Restaurant?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.NotFound);

            _restaurantRepository.DidNotReceive().Update(Arg.Any<Restaurant>());
        }

        [Fact]
        public async Task Handle_WhenRestaurantIsInactive_ShouldReturnFailure()
        {
            var restaurant = CreateRestaurant(active: false);
            var command = CreateValidCommand();

            _restaurantRepository.GetByIdAsync(command.Request.Id).Returns(restaurant);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.RestaurantInactive);
        }

        [Fact]
        public async Task Handle_WhenNameChangedAndAlreadyExists_ShouldReturnFailure()
        {
            var restaurant = CreateRestaurant();
            var command = CreateValidCommand(name: "New Name");

            _restaurantRepository.GetByIdAsync(command.Request.Id).Returns(restaurant);
            _restaurantRepository.AnyByNameAsync("New Name").Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.NameAlreadyExists);
        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ShouldUpdateRestaurantAndReturnSuccess()
        {
            var restaurant = CreateRestaurant();
            var command = CreateValidCommand(name: "Updated Name");

            _restaurantRepository.GetByIdAsync(command.Request.Id).Returns(restaurant);
            _restaurantRepository.AnyByNameAsync("Updated Name").Returns(false);

            var newAddress = new Address("New", "District", "City", 40, 28);
            _mapper.Map<Address>(command.Request.Address).Returns(newAddress);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be(RestaurantMessages.RestaurantUpdated);

            _restaurantRepository.Received(1).Update(restaurant);
        }

       
        private static Restaurant CreateRestaurant(bool active = true)
        {
            var restaurant = new Restaurant(
                "Old Name",
                new Address("Street", "District", "City", 41, 29));

            if (!active)
                restaurant.ToggleActive();

            return restaurant;
        }

        private static UpdateRestaurantCommand CreateValidCommand(string name = "Old Name")
        {
            return new UpdateRestaurantCommand(
                new UpdateRestaurantRequest
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
                });
        }

    }
}
