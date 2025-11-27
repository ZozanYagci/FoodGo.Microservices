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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDbContext _context;

        public CategoryRepository(CatalogDbContext context)
        {
            _context = context;
        }
        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();

        }

        public async Task<Category?> GetByIdAsync(Guid Id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
        }

        public IQueryable<Category> Query(bool tracking = false)
        {
            var query = _context.Categories.AsQueryable();

            return tracking ? query : query.AsNoTracking();

        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);

        }
    }
}
