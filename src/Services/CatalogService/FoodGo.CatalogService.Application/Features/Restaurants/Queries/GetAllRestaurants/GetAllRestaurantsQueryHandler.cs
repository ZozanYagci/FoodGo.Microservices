using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, List<GetRestaurantListItemResponse>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<List<GetRestaurantListItemResponse>> Handle(GetAllRestaurantsQuery query, CancellationToken cancellationToken)
        {
            var restaurants = await _restaurantRepository.GetAllAsync();

            return _mapper.Map<List<GetRestaurantListItemResponse>>(restaurants);
        }
    }
}
