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

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, DeletedRestaurantResponse>
    {
        private readonly IRestaurantRepository _restaurantRepository;


        public DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;

        }

        public async Task<DeletedRestaurantResponse> Handle(DeleteRestaurantCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(command.Request.Id);

            if (restaurant is null)
                throw new Exception(RestaurantMessages.RestaurantNotFound);

            _restaurantRepository.Delete(restaurant);

            var response = new DeletedRestaurantResponse
            {
                Id = restaurant.Id,
                Message = RestaurantMessages.RestaurantDeleted
            };

            return response;
        }
    }
}
