using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Restaurants.Dtos.Common.Models.Pagination
{
    public class PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public bool HasNext => Page * PageSize < TotalCount;
        public bool HasPrevious => Page > 1;

        private PagedResult(IReadOnlyCollection<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public static PagedResult<T> Create (IReadOnlyCollection<T> items, int page, int pageSize, int totalCount)
            => new PagedResult<T>(items, page, pageSize, totalCount);
    }
}
