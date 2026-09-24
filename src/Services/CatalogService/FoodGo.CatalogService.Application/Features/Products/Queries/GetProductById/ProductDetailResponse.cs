using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Features.Products.Queries.GetProductById
{
    public class ProductDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public Guid RestaurantId { get; set; }
        public bool IsActive { get; set; }


        public List<ProductPriceResponse> Prices { get; set; } = new();
        public List<ProductImageResponse> Images { get; set; } = new();
        public List<ProductOptionResponse> Options { get; set; } = new();

    }

    public class ProductPriceResponse
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!;
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }

    public class ProductImageResponse
    {
        public string Url { get; set; } = default!;
        public bool IsPrimary { get; set; }
    }

    public class ProductOptionResponse
    {
        public string Name { get; set; } = default!;
        public decimal AdditionalPrice { get; set; }
        public string Currency { get; set; } = default!;
    }

}
