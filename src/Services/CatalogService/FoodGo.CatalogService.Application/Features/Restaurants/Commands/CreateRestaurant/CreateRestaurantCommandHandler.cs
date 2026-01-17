using AutoMapper;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Rules;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<CreatedRestaurantResponse>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly RestaurantBusinessRules _businessRules;

        public CreateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IMapper mapper, RestaurantBusinessRules businessRules)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        public async Task<Result<CreatedRestaurantResponse>> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
        {
            var uniqueNameResult =
                await _businessRules.RestaurantNameMustBeUnique(command.Request.Name);

            if (uniqueNameResult.IsFailure)
                return Result<CreatedRestaurantResponse>
                    .Failure(uniqueNameResult.Error.Code);

            var restaurant = new Restaurant(
                command.Request.Name,
                _mapper.Map<Address>(command.Request.Address));

            _restaurantRepository.Add(restaurant);

            var response = _mapper.Map<CreatedRestaurantResponse>(restaurant);
            response.Message = RestaurantMessages.RestaurantCreated;

            return Result<CreatedRestaurantResponse>.Success(response);
        }
    }

}
