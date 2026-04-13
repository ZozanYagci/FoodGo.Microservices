using FoodGo.CatalogService.Application.Interfaces.Repositories;
using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {

        private readonly CatalogDbContext _context;

        public RestaurantRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public void Add(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            return await _context.Restaurants
                .AsNoTracking()
                .AnyAsync(r => r.Name == name);
        }

        public void Delete(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
        }

        public async Task<Restaurant?> GetByIdAsync(Guid Id, bool tracking = true)
        {
            var query = _context.Restaurants.AsQueryable();

            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(r => r.Id == Id);
        }
      
    }
}
