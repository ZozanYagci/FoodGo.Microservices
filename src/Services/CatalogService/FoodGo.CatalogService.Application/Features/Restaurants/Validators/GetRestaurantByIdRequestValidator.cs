using FluentValidation;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Validators
{
    public class GetRestaurantByIdRequestValidator : AbstractValidator<GetRestaurantByIdRequest>
    {
        public GetRestaurantByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Restaurant Id boş olamaz.");
        }
    }
}
