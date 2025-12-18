using FluentValidation;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Validators
{
    public class UpdateRestaurantRequestValidator : AbstractValidator<UpdateRestaurantRequest>
    {
        public UpdateRestaurantRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Restaurant Id boş olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Restaurant adı boş olamaz.")
                .MaximumLength(200);

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address!)
                .SetValidator(new AddressDtoValidator());
            });
        }
    }
}
