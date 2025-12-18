using FluentValidation;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Validators
{
    public class CreateRestaurantRequestValidator : AbstractValidator<CreateRestaurantRequest>
    {
        public CreateRestaurantRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Restoran adı boş olamaz.")
                .MaximumLength(200);

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Adres bilgisi zorunludur.")
                .SetValidator(new AddressDtoValidator());
        }
    }
}
