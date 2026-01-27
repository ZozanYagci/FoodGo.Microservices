using AutoMapper;
using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;

namespace FoodGo.CatalogService.Application.Tests.Features.Restaurants.Commands
{
    public class CreateRestaurantCommandHandlerTests
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly RestaurantBusinessRules _businessRules;
        private readonly CreateRestaurantCommandHandler _handler;

        public CreateRestaurantCommandHandlerTests()
        {
            _restaurantRepository = Substitute.For<IRestaurantRepository>();
            _mapper = Substitute.For<IMapper>();
            _businessRules = new RestaurantBusinessRules(_restaurantRepository);

            _handler = new CreateRestaurantCommandHandler(
                _restaurantRepository, _mapper, _businessRules);
        }

        [Fact]
        public async Task Handle_WhenRestaurantNameAlreadyExists_ShouldReturnFailure()
        {
            //Arrange
            var command = CreateValidCommand();

            _restaurantRepository
                .AnyByNameAsync(command.Request.Name)
                .Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.NameAlreadyExists);

            _restaurantRepository
               .DidNotReceive()
               .Add(Arg.Any<Restaurant>());

        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ShouldCreateRestaurantAndReturnSuccess()
        {
            // Arrange
            var command = CreateValidCommand();

            _restaurantRepository
                .AnyByNameAsync(command.Request.Name)
                .Returns(false);

            var address = new Address("Street", "District", "City", 41, 29);

            _mapper.Map<Address>(command.Request.Address).Returns(address);
            _mapper.Map<CreatedRestaurantResponse>(Arg.Any<Restaurant>())
                .Returns(new CreatedRestaurantResponse());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be(RestaurantMessages.RestaurantCreated);

            _restaurantRepository
               .Received(1)
               .Add(Arg.Any<Restaurant>());
        }

        private static CreateRestaurantCommand CreateValidCommand()
        {
            return new CreateRestaurantCommand(
                new CreateRestaurantRequest
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
                });
        }
    }
}