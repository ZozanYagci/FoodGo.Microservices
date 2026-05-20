using AutoMapper;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
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


        public GetRestaurantByIdQueryHandler(IRestaurantRepository repository)
        {
            _repository = repository;

        }

        public async Task<Result<GetRestaurantDetailResponse>> Handle(GetRestaurantByIdQuery query, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetByIdAsync(query.Id, tracking: false);

            if (restaurant is null)
                return Result<GetRestaurantDetailResponse>.Failure(
                    RestaurantErrors.NotFound(query.Id));

            var response = new GetRestaurantDetailResponse
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                IsActive = restaurant.IsActive,

                Address = new AddressDto
                {
                    Street = restaurant.Address.Street,
                    District = restaurant.Address.District,
                    City = restaurant.Address.City,
                    Latitude = restaurant.Address.Latitude,
                    Longitude = restaurant.Address.Longitude
                }
            };

            return Result<GetRestaurantDetailResponse>.Success(response);
        }
    }
}
