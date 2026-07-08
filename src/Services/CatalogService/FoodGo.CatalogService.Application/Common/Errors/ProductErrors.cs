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

        public static Error ImageCannotBeEmpty =>
            Error.Validation(
                "Product.Image.Empty",
                "Image url cannot be empty.");

        public static Error ImageAlreadyExists =>
            Error.Business(
                "Product.Image.Duplicate",
                "Image already exists.");

        public static Error OptionAlreadyExists =>
            Error.Business(
                "Product.Option.Duplicate",
                "Product option already exists.");

        public static Error NotFound(Guid id) =>
            Error.NotFound(
                "Product.NotFound",
                $"Product with id '{id}' not found.");
    }
}
