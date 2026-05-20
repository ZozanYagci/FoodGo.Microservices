using AutoMapper;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common.Models.Pagination;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurants;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetAllRestaurants
{
    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, Result<PagedResult<GetRestaurantsResponse>>>
    {
        private readonly IRestaurantRepository _repository;


        public GetRestaurantsQueryHandler(IRestaurantRepository repository)
        {
            _repository = repository;

        }

        public async Task<Result<PagedResult<GetRestaurantsResponse>>> Handle(GetRestaurantsQuery query, CancellationToken cancellationToken)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize > 50 ? 50 : query.PageSize;

            var skip = (page - 1) * pageSize;

            var totalCount = await _repository.CountAsync(cancellationToken);

            var restaurants = await _repository
                .GetPagedAsync(skip, pageSize, cancellationToken);

            var items = restaurants
                .Select(r => new GetRestaurantsResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToList();

            var pagedResult = PagedResult<GetRestaurantsResponse>
                .Create(items, page, pageSize, totalCount);

            return Result<PagedResult<GetRestaurantsResponse>>.Success(pagedResult);
        }
    }
}
