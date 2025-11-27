using FoodGo.CatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> Query(bool tracking = false);
        Task<Product?> GetByIdAsync(Guid Id);
        Task<List<Product>> GetAllAsync();
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
