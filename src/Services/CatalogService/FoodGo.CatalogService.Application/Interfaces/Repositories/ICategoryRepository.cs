using FoodGo.CatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Query(bool tracking = false);
        Task<Category?> GetByIdAsync(Guid Id);
        Task<List<Category>> GetAllAsync();
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);

    }
}
