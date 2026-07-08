using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurantById;
using FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurants;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            // entity --> response
            CreateMap<Restaurant, CreatedRestaurantResponse>();
            CreateMap<Restaurant, UpdatedRestaurantResponse>();

            CreateMap<Restaurant, GetRestaurantDetailResponse>();
            


            CreateMap<Restaurant, GetRestaurantDetailResponse>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));


            CreateMap<Address, AddressDto>();
        }
    }
}
