using FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public AddressDto? Address { get; set; }
        public bool IsActive { get; set; }
        
    }
}
