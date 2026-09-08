using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
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

namespace FoodGo.CatalogService.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreatedProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IRestaurantRepository restaurantRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _restaurantRepository = restaurantRepository;
            _categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<CreatedProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {


            var restaurantExists = await _restaurantRepository.ExistsAsync(
                command.RestaurantId, cancellationToken);

            if (!restaurantExists)
            {
                return Result<CreatedProductResponse>.Failure(
                    RestaurantErrors.NotFound(command.RestaurantId));
            }

            var categoryExists = await _categoryRepository.ExistsAsync(
                command.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Result<CreatedProductResponse>.Failure(
                    CategoryErrors.NotFound(command.CategoryId));
            }

            var productExists = await _productRepository.ExistsAsync(
                command.RestaurantId,
                command.Name,
                cancellationToken);

            if (productExists)
            {
                return Result<CreatedProductResponse>.Failure(
                    ProductErrors.NameAlreadyExists(command.Name));

            }

            var price = new Money(
                command.PriceAmount,
                command.Currency);

            var product = new Product(
                command.Name,
                command.Description,
                command.CategoryId,
                command.RestaurantId,
                price);

            _productRepository.Add(product);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new CreatedProductResponse
            {
                Id = product.Id,
                Name = product.Name
            };

            return Result<CreatedProductResponse>.Success(response);

        }
    }
}
