

using FoodGo.CatalogService.Application.Common.Results;
using MediatR;

namespace FoodGo.CatalogService.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Result<CreatedProductResponse>>
    {
        public string Name { get; init; } = default!;

        public string? Description { get; init; }

        public Guid CategoryId { get; init; }

        public Guid RestaurantId { get; init; }

        public decimal PriceAmount { get; init; }

        public string Currency { get; init; } = default!;
    }
}
