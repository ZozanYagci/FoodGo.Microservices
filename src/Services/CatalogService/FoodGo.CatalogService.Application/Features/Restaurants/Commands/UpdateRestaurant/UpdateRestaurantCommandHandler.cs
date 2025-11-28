using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
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
            var restaurant = await _restaurantRepository.GetByIdAsync(command.Request.Id);


            if (restaurant is null)
                throw new Exception(RestaurantMessages.RestaurantNotFound);

            _mapper.Map(command.Request, restaurant);

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
