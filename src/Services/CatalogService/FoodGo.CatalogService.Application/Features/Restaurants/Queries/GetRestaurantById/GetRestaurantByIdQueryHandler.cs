using AutoMapper;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, Result<GetRestaurantDetailResponse>>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper;

        public GetRestaurantByIdQueryHandler(IRestaurantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<GetRestaurantDetailResponse>> Handle(GetRestaurantByIdQuery query, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetByIdAsync(query.Id, tracking: false);

            if (restaurant is null)
                return Result<GetRestaurantDetailResponse>.Failure(
                    RestaurantErrors.NotFound(query.Id));

            var response = _mapper.Map<GetRestaurantDetailResponse>(restaurant);

            return Result<GetRestaurantDetailResponse>.Success(response);
        }
    }
}
