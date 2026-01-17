using FoodGo.CatalogService.Application.Common.Results;
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

        public Result RestaurantMustExist(Restaurant? restaurant)
        {
            if (restaurant is null)
                return Result.Failure(RestaurantErrors.RestaurantNotFound);

            return Result.Success();
        }

        public async Task<Result> RestaurantNameMustBeUnique(string name)
        {
            var exists = await _restaurantRepository.Query().AnyAsync(r => r.Name == name);

            if (exists)
                return Result.Failure(RestaurantErrors.NameAlreadyExists);

            return Result.Success();
        }

        public Result RestaurantMustBeActive(bool isActive)
        {
            if (!isActive)
                return Result.Failure(RestaurantErrors.RestaurantInactive);

            return Result.Success();
        }

        public Result CategoryCannotBeDuplicated(IEnumerable<string> existingCategories, string newCategory)
        {
            if (existingCategories.Any(c => c == newCategory))
                return Result.Failure(RestaurantErrors.CategoryAlreadyExist);

            return Result.Success();
        }

        public Result CategoryLimitCannotBeExceed(int currentCount, int maxLimit = 10)
        {
            if (currentCount >= maxLimit)
                return Result.Failure(RestaurantErrors.CategoryLimitExceeded);

            return Result.Success();
        }
    }
}
