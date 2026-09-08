using FoodGo.CatalogService.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Errors
{
    public static class ProductErrors
    {

        #region Validation

        public static Error NameCannotBeEmpty =>
            Error.Validation(
                "Product.Name.Empty",
                "Product name cannot be empty.");

        public static Error DescriptionTooLong =>
            Error.Validation(
                "Product.Description.TooLong",
                "Product description cannot exceed 1000 characters.");

        public static Error CategoryRequired =>
            Error.Validation(
                "Product.Category.Required",
                "Category is required.");

        public static Error RestaurantRequired =>
            Error.Validation(
                "Product.Restaurant.Required",
                "Restaurant is required.");

        #endregion

        #region Business

        public static Error NameAlreadyExists(string name) =>
            Error.Business(
                "Product.Name.Exists",
                $"A product named '{name}' already exists for this restaurant");

        public static Error RestaurantNotFound(Guid restaurantId) =>
            Error.Business(
                "Product.Restaurant.NotFound",
            $"Restaurant with id '{restaurantId}' was not found.");

        public static Error CategoryNotFound(Guid categoryId) =>
            Error.Business(
                "Product.Category.NotFound",
                $"Category with id '{categoryId}' was not found");

        #endregion

        #region NotFound

        public static Error NotFound(Guid id) =>
            Error.NotFound(
                "Product.NotFound",
                $"Product with id '{id}' not found.");
    }
}
#endregion