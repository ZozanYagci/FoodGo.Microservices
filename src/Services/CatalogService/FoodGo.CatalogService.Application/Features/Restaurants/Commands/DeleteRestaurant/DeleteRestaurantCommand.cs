using FoodGo.CatalogService.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand : IRequest<Result<DeletedRestaurantResponse>>
    {
        public Guid Id { get; set; }
    }
}
