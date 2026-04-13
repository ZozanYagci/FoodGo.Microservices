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
        Task<Restaurant?> GetByIdAsync(Guid Id, bool tracking=true);

        Task<bool> AnyByNameAsync(string name);
        void Add(Restaurant restaurant);
        void Delete(Restaurant restaurant);
    }
}
