using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Responses
{
    public class CreatedRestaurantResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = default!;
    }
}
