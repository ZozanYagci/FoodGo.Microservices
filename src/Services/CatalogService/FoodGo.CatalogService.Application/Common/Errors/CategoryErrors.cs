using FoodGo.CatalogService.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Errors
{
    public static class CategoryErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("Category.NotFound",
                $"Category with id '{id}' not found.");
    }
}
