using FoodGo.CatalogService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string Url { get; private set; }
        public bool IsPrimary { get; private set; }

        public ProductImage()
        {

        }

        public ProductImage(string url, bool isPrimary = false)
        {
            Url = string.IsNullOrWhiteSpace(url) ? throw new ArgumentException("Resim url boş olamaz.", nameof(url)) : url;
            IsPrimary = isPrimary;
        }

        public void MarkPrimary()
        {
            IsPrimary = true;
        }

        public void UnmarkPrimary()
        {
            IsPrimary = false;
        }
    }
}
