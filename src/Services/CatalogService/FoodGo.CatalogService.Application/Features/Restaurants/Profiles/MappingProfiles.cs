using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Domain.Entities;
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
            // request --> entity
            CreateMap<CreateRestaurantRequest, Restaurant>();
            CreateMap<UpdateRestaurantRequest, Restaurant>();


            // entity --> response
            CreateMap<Restaurant, CreatedRestaurantResponse>();
            CreateMap<Restaurant, UpdatedRestaurantResponse>();

            CreateMap<Restaurant, GetRestaurantDetailResponse>();
            CreateMap<Restaurant, GetRestaurantListItemResponse>();
        }
    }
}
