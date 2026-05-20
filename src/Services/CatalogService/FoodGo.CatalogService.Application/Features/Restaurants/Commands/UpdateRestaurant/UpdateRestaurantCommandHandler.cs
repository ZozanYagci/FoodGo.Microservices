using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.ValueObjects;
using MediatR;


namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result<UpdatedRestaurantResponse>>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRestaurantCommandHandler(IRestaurantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;

        }

        public async Task<Result<UpdatedRestaurantResponse>> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetByIdAsync(command.Id, tracking: true);

            if (restaurant is null)
                return Result<UpdatedRestaurantResponse>.Failure(
                    RestaurantErrors.NotFound(command.Id));

            if (!restaurant.IsActive)
                return Result<UpdatedRestaurantResponse>.Failure(
                    RestaurantErrors.RestaurantInactive);

            if (!string.IsNullOrWhiteSpace(command.Name) &&
                restaurant.Name != command.Name)
            {
                if (await _repository.AnyByNameAsync(command.Name))
                    return Result<UpdatedRestaurantResponse>.Failure(
                        RestaurantErrors.NameAlreadyExists(command.Name));

                restaurant.SetName(command.Name);
            }

            if (command.Address is not null)
            {
                var address = new Address(
                    command.Address.Street,
                    command.Address.District,
                    command.Address.City,
                    command.Address.Latitude,
                    command.Address.Longitude);

                restaurant.SetAddress(address);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<UpdatedRestaurantResponse>.Success(
            new UpdatedRestaurantResponse
            {
                Id = command.Id,
                Name=restaurant.Name
            });

        }
    }
}
