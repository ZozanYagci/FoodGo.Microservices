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
       
        Task<Product?> GetByIdAsync(Guid Id, CancellationToken cancellationToken=default);

        Task<bool> ExistsAsync(Guid restaurantId, string name, CancellationToken cancellationToken=default);
        
        void Add(Product product);
       
    }
}
