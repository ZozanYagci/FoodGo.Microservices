using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand : IRequest<DeletedRestaurantResponse>
    {
        public DeleteRestaurantRequest Request { get; set; } = default!;

        public DeleteRestaurantCommand(DeleteRestaurantRequest request)
        {
            Request = request;
        }
    }
}
