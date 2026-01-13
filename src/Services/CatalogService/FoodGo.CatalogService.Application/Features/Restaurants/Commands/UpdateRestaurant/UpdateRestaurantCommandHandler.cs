using AutoMapper;
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
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, UpdatedRestaurantResponse>
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

        public async Task<UpdatedRestaurantResponse> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
        {

            var request = command.Request;
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);

            _businessRules.RestaurantMustExist(restaurant);
            _businessRules.RestaurantMustBeActive(restaurant.IsActive);

            if (restaurant.Name != request.Name)
            {
                await _businessRules.RestaurantNameMustBeUnique(request.Name);
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

            return response;

        }
    }
}
