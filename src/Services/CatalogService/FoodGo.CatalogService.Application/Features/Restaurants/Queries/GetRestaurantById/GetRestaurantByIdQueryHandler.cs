using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, GetRestaurantDetailResponse>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetRestaurantByIdQueryHandler(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<GetRestaurantDetailResponse> Handle(GetRestaurantByIdQuery query, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(query.Request.Id);

            if (restaurant is null)
                throw new Exception("Restaurant bulunamadı.");

            var response = _mapper.Map<GetRestaurantDetailResponse>(restaurant);
            response.Message = "Restaurant detayları getirildi.";

            return response;
        }
    }
}
