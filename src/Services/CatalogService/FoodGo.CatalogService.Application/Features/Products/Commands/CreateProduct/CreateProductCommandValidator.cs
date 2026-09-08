using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name cannot be empty.")
                .MaximumLength(200)
                .WithMessage("Product name cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Product description cannot exceed 1000 characters.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category is required.");

            RuleFor(x => x.RestaurantId)
                .NotEmpty()
                .WithMessage("Restaurant is required.");

            RuleFor(x => x.PriceAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product price cannot be negative.");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency is required.")
                .Length(3)
                .WithMessage("Currency must be a 3-letter code.");
        }
    }
}
