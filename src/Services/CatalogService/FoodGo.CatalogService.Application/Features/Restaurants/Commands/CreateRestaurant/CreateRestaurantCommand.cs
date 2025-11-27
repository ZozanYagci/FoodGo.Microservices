using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : IRequest<CreatedRestaurantResponse>
    {
        public CreateRestaurantRequest Request { get; set; } = null!;

        public CreateRestaurantCommand(CreateRestaurantRequest request)
        {
            Request = request;
        }
    }
}
