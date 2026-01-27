using FoodGo.CatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Interfaces.Repositories
{
    public interface IRestaurantRepository
    {
        IQueryable<Restaurant> Query(bool tracking = false);
        Task<Restaurant?> GetByIdAsync(Guid Id);
        Task<List<Restaurant>> GetAllAsync();

        Task<bool> AnyByNameAsync(string name);
        void Add(Restaurant restaurant);
        void Update(Restaurant restaurant);
        void Delete(Restaurant restaurant);
    }
}
