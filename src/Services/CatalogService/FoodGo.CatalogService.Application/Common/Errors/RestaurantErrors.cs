using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Domain.SeedWork.DomainErrors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Errors
{
    public static class RestaurantErrors
    {
        public static Error NameCannotBeEmpty =>
            Error.Business(
                RestaurantRules.NameCannotBeEmpty,
                "Restaurant name cannot be empty.");

        public static Error AddressCannotBeNull =>
            Error.Business(
                RestaurantRules.AddressCannotBeNull,
                "Restaurant address cannot be null.");

        public static Error CategoryAlreadyExist =>
            Error.Business(
                RestaurantRules.CategoryAlreadyExist,
                "This category already exists.");

        public static Error CategoryLimitExceeded =>
            Error.Business(
                RestaurantRules.CategoryLimitExceeded,
                "Category limit has been exceeded.");

        public static Error RestaurantInactive =>
            Error.Business(
                RestaurantRules.RestaurantInactive,
                "Restaurant is not active.");

        public static Error NotFound =>
            Error.Business(
                "Restaurant.NotFound",
                "Restaurant not found.");

        public static Error NameAlreadyExists =>
            Error.Business(
                "Restaurant.Name.AlreadyExists",
                "Name already exists");

    }
}
