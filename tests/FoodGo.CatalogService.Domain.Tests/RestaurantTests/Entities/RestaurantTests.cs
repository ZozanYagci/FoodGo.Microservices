using FluentAssertions;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.SeedWork.DomainErrors;
using FoodGo.CatalogService.Domain.ValueObjects;


namespace FoodGo.CatalogService.Domain.Tests.RestaurantTests.Entities
{
    public class RestaurantTests
    {
        private static Address CreateValidAddress()
            => new Address("Street", "District", "City", 41.0, 29.0);


        [Fact]
        public void Constructor_should_throw_DomainException_when_name_is_empty()
        {
            var address = CreateValidAddress();

            Action act = () => new Restaurant("", address);

            act.Should()
                .Throw<DomainException>()
                .Which.ErrorCode.Should()
                .Be(RestaurantRules.NameCannotBeEmpty);

        }

        [Fact]
        public void Constructor_should_throw_DomainException_when_address_is_null()
        {
            Action act = () => new Restaurant("Hevi Restaurant", null);

            act.Should()
                .Throw<DomainException>()
                .Which.ErrorCode.Should()
                .Be(RestaurantRules.AddressCannotBeNull);
        }

        [Fact]
        public void Constructor_should_create_restaurant_when_parameters_are_valid()
        {
            var address = CreateValidAddress();

            var restaurant = new Restaurant("Hevi Restaurant", address);

            restaurant.Name.Should().Be("Hevi Restaurant");
            restaurant.Address.Should().Be(address);
            restaurant.IsActive.Should().BeTrue();
        }


        [Fact]
        public void UpdateName_should_throw_DomainException_when_new_name_is_empty()
        {
            var restaurant = new Restaurant("Old name", CreateValidAddress());

            Action act = () => restaurant.UpdateName("");

            act.Should()
                .Throw<DomainException>()
                .Which.ErrorCode.Should()
                .Be(RestaurantRules.NameCannotBeEmpty);
        }

        [Fact]
        public void UpdateName_should_do_nothing_when_name_is_same()
        {
            var restaurant = new Restaurant("Same Name", CreateValidAddress());

            restaurant.UpdateName("Same Name");

            restaurant.Name.Should().Be("Same Name");
        }

        [Fact]
        public void UpdateName_should_update_name_when_new_name_is_different()
        {
            var restaurant = new Restaurant("Old Name", CreateValidAddress());

            restaurant.UpdateName("New Name");

            restaurant.Name.Should().Be("New Name");
        }

        [Fact]
        public void UpdateName_should_set_UpdatedAt_when_name_changes()
        {
            var restaurant = new Restaurant("Old Name", CreateValidAddress());

            restaurant.UpdatedAt.Should().BeNull();

            restaurant.UpdateName("New Name");

            restaurant.UpdatedAt.Should().NotBeNull();
        }


        [Fact]
        public void UpdateAddress_should_throw_DomainException_when_address_is_null()
        {
            var restaurant = new Restaurant("Restaurant", CreateValidAddress());

            Action act = () => restaurant.UpdateAddress(null);

            act.Should()
                .Throw<DomainException>()
                .Which.ErrorCode.Should()
                .Be(RestaurantRules.AddressCannotBeNull);
        }

        [Fact]
        public void UpdateAddress_should_update_address_when_valid()
        {
            var restaurant = new Restaurant("Restaurant", CreateValidAddress());
            var newAddress = new Address("New St", "New Dist", "New City", 40, 28);

            restaurant.UpdateAddress(newAddress);

            restaurant.Address.Should().Be(newAddress);

        }

        [Fact]
        public void AddCategory_should_throw_DomainException_when_restaurant_is_inactive()
        {

            var restaurant = new Restaurant("Restaurant", CreateValidAddress());
            restaurant.ToggleActive();
            var categoryId = Guid.NewGuid();

            Action act = () => restaurant.AddCategory(categoryId);

            act.Should()
               .Throw<DomainException>()
               .Which.ErrorCode.Should()
               .Be(RestaurantRules.RestaurantInactive);
        }

        [Fact]
        public void AddCategory_should_throw_DomainException_when_category_already_exists()
        {

            var restaurant = new Restaurant("Restaurant", CreateValidAddress());
            var categoryId = Guid.NewGuid();

            restaurant.AddCategory(categoryId);

            Action act = () => restaurant.AddCategory(categoryId);

            act.Should()
               .Throw<DomainException>()
               .Which.ErrorCode.Should()
               .Be(RestaurantRules.CategoryAlreadyExist);
        }

        [Fact]
        public void AddCategory_should_add_category_when_valid()
        {

            var restaurant = new Restaurant("Restaurant", CreateValidAddress());
            var categoryId = Guid.NewGuid();

            restaurant.AddCategory(categoryId);

            restaurant.CategoryIds.Should().Contain(categoryId);
        }

        [Fact]
        public void ToggleActive_should_change_IsActive_state()
        {

            var restaurant = new Restaurant("Restaurant", CreateValidAddress());

            restaurant.ToggleActive();

            restaurant.IsActive.Should().BeFalse();
        }

    }
}
