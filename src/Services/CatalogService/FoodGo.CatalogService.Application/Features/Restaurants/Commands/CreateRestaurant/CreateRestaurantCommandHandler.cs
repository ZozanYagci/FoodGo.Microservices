using AutoMapper;
using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<CreatedRestaurantResponse>>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRestaurantCommandHandler(IRestaurantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreatedRestaurantResponse>> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
        {
            if (await _repository.AnyByNameAsync(command.Name))
                return Result<CreatedRestaurantResponse>.Failure(
                    RestaurantErrors.NameAlreadyExists(command.Name));

            var address = new Address(
                command.Address.Street,
                command.Address.District,
                command.Address.City,
                command.Address.Latitude,
                command.Address.Longitude
                );

            var restaurant = new Restaurant(command.Name, address);

            _repository.Add(restaurant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new CreatedRestaurantResponse
            {
                Id = restaurant.Id,
                Name = restaurant.Name
            };
            return Result<CreatedRestaurantResponse>.Success(response);
        }
    }

}
