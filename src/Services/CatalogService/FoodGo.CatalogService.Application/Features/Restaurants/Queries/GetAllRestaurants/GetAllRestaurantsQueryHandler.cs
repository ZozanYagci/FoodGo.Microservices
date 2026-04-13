using AutoMapper;
using FoodGo.CatalogService.Application.Common.Results;
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
    //public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, Result<List<GetRestaurantListItemResponse>>>
    //{
    //    private readonly IRestaurantRepository _repository;
    //    private readonly IMapper _mapper;

    //    public GetAllRestaurantsQueryHandler(IRestaurantRepository repository, IMapper mapper)
    //    {
    //        _repository = repository;
    //        _mapper = mapper;
    //    }

    //    public async Task<Result<List<GetRestaurantListItemResponse>>> Handle(GetAllRestaurantsQuery query, CancellationToken cancellationToken)
    //    {
    //        var restaurants = await _repository.GetAllAsync();

    //        var response= _mapper.Map<List<GetRestaurantListItemResponse>>(restaurants);

    //        return Result<List<GetRestaurantListItemResponse>>.Success(response);
    //    }
    //}


}
