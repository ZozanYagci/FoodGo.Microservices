using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
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

        public UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<UpdatedRestaurantResponse> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
        {

            var request = command.Request;
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);


            if (restaurant is null)
                throw new Exception(RestaurantMessages.RestaurantNotFound);

            restaurant.UpdateName(request.Name);

            if (request.Address != null)
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
