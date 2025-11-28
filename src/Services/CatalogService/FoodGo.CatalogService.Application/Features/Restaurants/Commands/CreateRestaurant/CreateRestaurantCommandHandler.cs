using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Constants;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, CreatedRestaurantResponse>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public CreateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<CreatedRestaurantResponse> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
        {
            var restaurant = _mapper.Map<Restaurant>(command.Request);

            _restaurantRepository.Add(restaurant);

            var response = _mapper.Map<CreatedRestaurantResponse>(restaurant);
            response.Message = RestaurantMessages.RestaurantCreated;

            return response;
        }
    }
}
