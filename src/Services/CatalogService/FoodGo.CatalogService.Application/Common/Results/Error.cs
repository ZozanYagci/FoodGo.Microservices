using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Results
{
    public sealed record Error(string Code)
    {
        public static readonly Error None = new(string.Empty);
    }
}
