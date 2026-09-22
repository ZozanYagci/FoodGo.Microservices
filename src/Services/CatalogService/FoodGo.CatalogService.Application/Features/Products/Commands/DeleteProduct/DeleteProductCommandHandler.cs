using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Interfaces;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeletedProductResponse>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DeletedProductResponse>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(
                command.Id, cancellationToken);

            if (product is null)
            {
                return Result<DeletedProductResponse>.Failure(
                    ProductErrors.NotFound(command.Id));
            }

            product.Delete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new DeletedProductResponse
            {
                Id = product.Id
            };

            return Result<DeletedProductResponse>.Success(response);
        }
    }
}
