using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;


namespace FoodGo.CatalogService.Application.Tests.Features.Restaurants.Commands
{
    public class DeleteRestaurantCommandHandlerTests
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly RestaurantBusinessRules _businessRules;
        private readonly DeleteRestaurantCommandHandler _handler;

        public DeleteRestaurantCommandHandlerTests()
        {
            _restaurantRepository = Substitute.For<IRestaurantRepository>();
            _businessRules = new RestaurantBusinessRules(_restaurantRepository);

            _handler = new DeleteRestaurantCommandHandler(
                _restaurantRepository, _businessRules);
        }

        [Fact]
        public async Task Handle_WhenRestaurantNotFound_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _restaurantRepository.GetByIdAsync(command.Request.Id)
                .Returns((Restaurant?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.NotFound);

            _restaurantRepository.DidNotReceive().Delete(Arg.Any<Restaurant>());
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
        public async Task Handle_WhenRequestIsValid_ShouldDeleteRestaurantAndReturnSuccess()
        {
            var restaurant = CreateRestaurant();
            var command = CreateValidCommand();

            _restaurantRepository.GetByIdAsync(command.Request.Id).Returns(restaurant);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be(RestaurantMessages.RestaurantDeleted);

            _restaurantRepository.Received(1).Delete(restaurant);
        }

        private static Restaurant CreateRestaurant(bool active = true)
        {
            var restaurant = new Restaurant(
                "Restaurant",
                new Address("Street", "District", "City", 41, 29));

            if (!active)
                restaurant.ToggleActive();

            return restaurant;
        }

        private static DeleteRestaurantCommand CreateValidCommand()
        {
            return new DeleteRestaurantCommand(
                new DeleteRestaurantRequest
                {
                    Id = Guid.NewGuid()
                });
        }
    }
}
