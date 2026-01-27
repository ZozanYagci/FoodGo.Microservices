using FluentAssertions;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using NSubstitute;



namespace FoodGo.CatalogService.Application.Tests.Features.Restaurants.Rules
{
    public class RestaurantBusinessRulesTests
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly RestaurantBusinessRules _rules;

        public RestaurantBusinessRulesTests()
        {
            _restaurantRepository = Substitute.For<IRestaurantRepository>();
            _rules = new RestaurantBusinessRules(_restaurantRepository);
        }

        [Fact]
        public void RestaurantMustExist_WhenRestaurantIsNull_ShouldReturnFailure()
        {
            var result = _rules.RestaurantMustExist(null);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.NotFound);
        }

        [Fact]
        public void RestaurantMustExist_WhenRestaurantExist_ShouldReturnSuccess()
        {
            var restaurant = new Restaurant(
                "Hevi Restaurant", new Address("Street", "District", "City", 41, 29));

            var result = _rules.RestaurantMustExist(restaurant);

            result.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task RestaurantNameMustBeUnique_WhenNameExists_ShouldReturnFailure()
        {

            _restaurantRepository.AnyByNameAsync("Pizza House").Returns(true);

            var result = await _rules.RestaurantNameMustBeUnique("Pizza House");
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.NameAlreadyExists);
        }

        [Fact]
        public async Task RestaurantNameMustBeUnique_WhenNameIsUnique_ShouldReturnSuccess()
        {

            _restaurantRepository.AnyByNameAsync("Sushi Bar").Returns(false);

            var result = await _rules.RestaurantNameMustBeUnique("Sushi Bar");

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void RestaurantMustBeActive_WhenInactive_ShouldReturnFailure()
        {
            var result = _rules.RestaurantMustBeActive(false);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.RestaurantInactive);
        }

        [Fact]
        public void RestaurantMustBeActive_WhenActive_ShouldReturnSuccess()
        {
            var result = _rules.RestaurantMustBeActive(true);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CategoryCannotBeDuplicated_WhenCategoryExists_ShouldReturnFailure()
        {
            var categories = new List<string> { "FastFood", "Dessert" };

            var result = _rules.CategoryCannotBeDuplicated(categories, "Dessert");

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.CategoryAlreadyExist);
        }

        [Fact]
        public void CategoryCannotBeDuplicated_WhenCategoryIsNew_ShouldReturnSuccess()
        {

            var categories = new List<string> { "FastFood", "Dessert" };

            var result = _rules.CategoryCannotBeDuplicated(categories, "Drinks");

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CategoryLimitCannotBeExceed_WhenLimitExceeded_ShouldReturnFailure()
        {
            var result = _rules.CategoryLimitCannotBeExceed(10);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == RestaurantErrors.CategoryLimitExceeded);
        }

        [Fact]
        public void CategoryLimitCannotBeExceed_WhenBelowLimit_ShouldReturnSuccess()
        {
            var result = _rules.CategoryLimitCannotBeExceed(5);

            result.IsSuccess.Should().BeTrue();
        }
    }
}

