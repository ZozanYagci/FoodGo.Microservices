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
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<bool> ExistsAsync(Guid restaurantId, string name, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AnyAsync(
                p => p.RestaurantId == restaurantId &&
                p.Name == name, cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(p => p.Prices)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == Id, cancellationToken);
        }
    }
}
