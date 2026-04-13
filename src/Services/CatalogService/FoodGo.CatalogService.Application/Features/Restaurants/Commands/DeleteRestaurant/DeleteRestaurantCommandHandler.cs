using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Result<DeletedRestaurantResponse>>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IUnitOfWork _unitOfWork; 

        public DeleteRestaurantCommandHandler(IRestaurantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DeletedRestaurantResponse>> Handle(DeleteRestaurantCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetByIdAsync(command.Id, tracking: true);

            if(restaurant is null)
                return Result<DeletedRestaurantResponse>.Failure(
                    RestaurantErrors.NotFound(command.Id));

            if (!restaurant.IsActive)
                return Result<DeletedRestaurantResponse>.Failure(
                    RestaurantErrors.RestaurantInactive);

            _repository.Delete(restaurant);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<DeletedRestaurantResponse>.Success(
                new DeletedRestaurantResponse
                {
                    Id = restaurant.Id,
                });
        }
    }
}
