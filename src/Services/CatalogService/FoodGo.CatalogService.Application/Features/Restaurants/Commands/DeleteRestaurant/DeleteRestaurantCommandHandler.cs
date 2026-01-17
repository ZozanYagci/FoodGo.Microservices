using AutoMapper;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Result<DeletedRestaurantResponse>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly RestaurantBusinessRules _businessRules;

        public DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository, RestaurantBusinessRules businessRules)
        {
            _restaurantRepository = restaurantRepository;
            _businessRules = businessRules;
        }

        public async Task<Result<DeletedRestaurantResponse>> Handle(DeleteRestaurantCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(command.Request.Id);

            var existResult = _businessRules.RestaurantMustExist(restaurant);
            if (existResult.IsFailure)
                return Result<DeletedRestaurantResponse>.Failure(existResult.Error.Code);

            var activeResult = _businessRules.RestaurantMustBeActive(restaurant!.IsActive);
            if (activeResult.IsFailure)
                return Result<DeletedRestaurantResponse>.Failure(activeResult.Error.Code);


            _restaurantRepository.Delete(restaurant);

            var response = new DeletedRestaurantResponse
            {
                Id = restaurant.Id,
                Message = RestaurantMessages.RestaurantDeleted
            };

            return Result<DeletedRestaurantResponse>.Success(response);
        }
    }
}
