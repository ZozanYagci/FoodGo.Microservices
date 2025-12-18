using FluentValidation;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage("Sokak bilgisi boş olamaz")
                .MaximumLength(200);

            RuleFor(x => x.District)
                .NotEmpty()
                .WithMessage("İlçe bilgisi boş olamaz")
                .MaximumLength(150);

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Şehir bilgisi boş olamaz")
                .MaximumLength(150);

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude -90 ile 90 arasında olmalıdır.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude -180 ile 180 arasında olmalıdır.");


        }
    }
}
