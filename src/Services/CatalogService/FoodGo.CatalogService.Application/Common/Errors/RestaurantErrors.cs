using FoodGo.CatalogService.Application.Common.Results;

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
            Error.Validation(
                "Restaurant.Name.Empty",
                "Restaurant name cannot be empty.");

        public static Error AddressCannotBeNull =>
            Error.Validation(
                 "Restaurant.Address.Null",
                "Restaurant address cannot be null.");

        public static Error RestaurantInactive =>
             Error.Business(
                "Restaurant.Inactive",
                 "Restaurant is not active.");

        public static Error NotFound(Guid id) =>
            Error.NotFound(
                "Restaurant.NotFound",
                $"Restaurant with id '{id}' not found.");

        public static Error NameAlreadyExists(string name) =>
            Error.Business(
               "Restaurant.Name.Exists",
                $"Restaurant name '{name}' already exists.");

    }
}
