using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommand : IRequest<UpdatedRestaurantResponse>
    {
        public UpdateRestaurantRequest Request { get; set; } = default!;

        public UpdateRestaurantCommand(UpdateRestaurantRequest request)
        {
            Request = request;
        }
    }
}
