using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IRequest<GetRestaurantDetailResponse>
    {
        public GetRestaurantByIdRequest Request { get; set; } = default!;

        public GetRestaurantByIdQuery(GetRestaurantByIdRequest request)
        {
            Request = request;
        }
    }
}
