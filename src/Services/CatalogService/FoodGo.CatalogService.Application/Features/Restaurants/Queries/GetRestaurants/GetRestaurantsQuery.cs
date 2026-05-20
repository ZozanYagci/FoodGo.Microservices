using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common.Models.Pagination;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetAllRestaurants
{
    public class GetRestaurantsQuery : IRequest<Result<PagedResult<GetRestaurantsResponse>>>
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
