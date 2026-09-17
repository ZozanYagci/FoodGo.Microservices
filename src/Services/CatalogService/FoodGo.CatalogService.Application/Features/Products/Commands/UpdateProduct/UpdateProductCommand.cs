using FoodGo.CatalogService.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result<UpdatedProductResponse>>
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
    }
}
