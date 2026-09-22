using FoodGo.CatalogService.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result<DeletedProductResponse>>
    {
        public Guid Id { get; init; }
    }
}
