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

        public void Delete(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.AsNoTracking().ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(Guid Id)
        {
            return await _context.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Id == Id);
        }

        public IQueryable<Restaurant> Query(bool tracking = false)
        {
            var query = _context.Restaurants.AsQueryable();
            return tracking ? query : query.AsNoTracking();
        }

        public void Update(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);

        }
    }
}
