using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.SeedWork;
using FoodGo.CatalogService.Domain.SeedWork.DomainErrors;
using Microsoft.EntityFrameworkCore;


namespace FoodGo.CatalogService.Application.Features.Restaurants.Rules
{
    public class RestaurantBusinessRules
    {
        public readonly IRestaurantRepository _restaurantRepository;

        public RestaurantBusinessRules(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;

        }

        public void RestaurantMustExist(Restaurant? restaurant)
        {
            if (restaurant is null)
                throw new DomainException(RestaurantErrors.RestaurantNotFound);
        }

        public async Task RestaurantNameMustBeUnique(string name)
        {
            var exists = await _restaurantRepository.Query().AnyAsync(r => r.Name == name);

            if (exists)
                throw new DomainException(RestaurantErrors.NameAlreadyExists);
        }

        public void RestaurantMustBeActive(bool isActive)
        {
            if (!isActive)
                throw new DomainException(RestaurantErrors.RestaurantInactive);
        }

        public void CategoryCannotBeDuplicated(IEnumerable<string> existingCategories, string newCategory)
        {
            if (existingCategories.Any(c => c == newCategory))
                throw new DomainException(RestaurantErrors.CategoryAlreadyExist);
        }

        public void CategoryLimitCannotBeExceed(int currentCount, int maxLimit = 10)
        {
            if (currentCount >= maxLimit)
                throw new DomainException(RestaurantErrors.CategoryLimitExceeded);
        }
    }
}
