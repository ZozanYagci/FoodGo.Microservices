using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant
{
    public class DeletedRestaurantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
