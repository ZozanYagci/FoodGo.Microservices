using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Requests
{
    public class GetRestaurantByIdRequest
    {
        public Guid Id { get; set; }
    }
}
