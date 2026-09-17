using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<UpdatedProductResponse>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdatedProductResponse>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {

            var product = await _productRepository.GetByIdAsync(
                command.Id,
                cancellationToken);

            if (product is null)

                return Result<UpdatedProductResponse>.Failure(
                    ProductErrors.NotFound(command.Id));

            var nameAlreadyExists = await _productRepository.ExistsAsync(
                product.RestaurantId,
                command.Name,
                product.Id,
                cancellationToken);

            if (nameAlreadyExists)
            {
                return Result<UpdatedProductResponse>.Failure(
                    ProductErrors.NameAlreadyExists(command.Name));
            }

            product.SetName(command.Name);
            product.UpdateDescription(command.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UpdatedProductResponse
            {

                Id = product.Id,
                Name = product.Name

            };

            return Result<UpdatedProductResponse>.Success(response);


        }
    }
}
