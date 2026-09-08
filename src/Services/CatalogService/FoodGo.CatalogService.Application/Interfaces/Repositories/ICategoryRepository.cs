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

        Task<Category?> GetByIdAsync(Guid Id);

        Task<bool> ExistsAsync(Guid categoryId, CancellationToken cancellationToken = default);

        void Add(Category category);

    }
}
