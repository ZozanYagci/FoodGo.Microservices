using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;


namespace FoodGo.CatalogService.Application.Tests.Features.Restaurants.Commands
{
    public class DeleteRestaurantCommandHandlerTests
    {
        private readonly IRestaurantRepository _repository;
        private readonly DeleteRestaurantCommandHandler _handler;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRestaurantCommandHandlerTests()
        {
            _repository = Substitute.For<IRestaurantRepository>();
           _unitOfWork=Substitute.For<IUnitOfWork>();
            _handler = new DeleteRestaurantCommandHandler(
                _repository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_WhenRestaurantNotFound_ShouldReturnFailure()
        {
            var command = CreateValidCommand();

            _repository.GetByIdAsync(command.Id)
                .Returns((Restaurant?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            _repository.DidNotReceive().Delete(Arg.Any<Restaurant>());

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
        public async Task Handle_WhenRequestIsValid_ShouldDeleteRestaurantAndReturnSuccess()
        {
            var restaurant = CreateRestaurant();
            var command = CreateValidCommand();

            _repository.GetByIdAsync(command.Id).Returns(restaurant);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
           

            _repository.Received(1).Delete(restaurant);

            await _unitOfWork.Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        private static Restaurant CreateRestaurant(bool active = true)
        {
            var restaurant = new Restaurant(
                "Restaurant",
                new Address("Street", "District", "City", 41, 29));

            if (!active)
                restaurant.Deactivate();

            return restaurant;
        }

        private static DeleteRestaurantCommand CreateValidCommand()
        {
            return new DeleteRestaurantCommand
                {
                    Id = Guid.NewGuid()
                };
        }
    }
}
