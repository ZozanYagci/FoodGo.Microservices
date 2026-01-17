using AutoMapper;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result<UpdatedRestaurantResponse>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly RestaurantBusinessRules _businessRules;

        public UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IMapper mapper, RestaurantBusinessRules businessRules)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        public async Task<Result<UpdatedRestaurantResponse>> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
        {

            var request = command.Request;
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);

            var existResult = _businessRules.RestaurantMustExist(restaurant);

            if (existResult.IsFailure)
                return Result<UpdatedRestaurantResponse>.Failure(existResult.Error.Code);

            var activeResult = _businessRules.RestaurantMustBeActive(restaurant!.IsActive);

            if (activeResult.IsFailure)
                return Result<UpdatedRestaurantResponse>.Failure(activeResult.Error.Code);


            if (restaurant.Name != request.Name)
            {
                var uniqueResult = await _businessRules.RestaurantNameMustBeUnique(request.Name);

                if (uniqueResult.IsFailure)
                    return Result<UpdatedRestaurantResponse>.Failure(uniqueResult.Error.Code);

                restaurant.UpdateName(request.Name);
            }

            if (request.Address is not null)
            {
                var newAddress = _mapper.Map<Address>(request.Address);
                restaurant.UpdateAddress(newAddress);
            }

            _restaurantRepository.Update(restaurant);


            var response = new UpdatedRestaurantResponse
            {
                Id = restaurant.Id,
                Message = RestaurantMessages.RestaurantUpdated
            };

            return Result<UpdatedRestaurantResponse>.Success(response);

        }
    }
}
