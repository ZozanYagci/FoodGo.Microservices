using FoodGo.CatalogService.Application.Common.Errors;
using FoodGo.CatalogService.Application.Common.Results;
using FoodGo.CatalogService.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDetailResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDetailResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(
                query.Id, cancellationToken);

            if (product is null)
            {
                return Result<ProductDetailResponse>.Failure(
                    ProductErrors.NotFound(query.Id));
            }

            var response = new ProductDetailResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                RestaurantId = product.RestaurantId,
                IsActive = product.IsActive,

                Prices = product.Prices
                .Select(price => new ProductPriceResponse
                {
                    Amount = price.Price.Amount,
                    Currency = price.Price.Currency,
                    From = price.From,
                    To = price.To
                }).ToList(),

                Images = product.Images
                .Select(image => new ProductImageResponse
                {
                    Url = image.Url,
                    IsPrimary = image.IsPrimary
                }).ToList(),

                Options = product.Options
                .Select(option => new ProductOptionResponse
                {
                    Name = option.Name,
                    AdditionalPrice = option.AdditionalPrice.Amount,
                    Currency = option.AdditionalPrice.Currency
                }).ToList()
            };

            return Result<ProductDetailResponse>.Success(response);
        }
    }
}
