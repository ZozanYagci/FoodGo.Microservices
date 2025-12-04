using AutoMapper;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
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
            // request --> entity
            CreateMap<CreateRestaurantRequest, Restaurant>();
            CreateMap<UpdateRestaurantRequest, Restaurant>();


            // entity --> response
            CreateMap<Restaurant, CreatedRestaurantResponse>();
            CreateMap<Restaurant, UpdatedRestaurantResponse>();

            CreateMap<Restaurant, GetRestaurantDetailResponse>();
            CreateMap<Restaurant, GetRestaurantListItemResponse>();

            //
            CreateMap<AddressDto, Address>();

            CreateMap<CreateRestaurantRequest, Restaurant>()
                .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address));

            CreateMap<UpdateRestaurantRequest, Restaurant>()
                 .ForMember(dest => dest.Address, opt => opt.Ignore());

            CreateMap<Restaurant, GetRestaurantDetailResponse>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<Restaurant, GetRestaurantListItemResponse>()
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

        }
    }
}
