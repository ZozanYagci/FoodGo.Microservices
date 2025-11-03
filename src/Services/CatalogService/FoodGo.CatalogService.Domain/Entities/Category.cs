using FoodGo.CatalogService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Entities
{
    // DDD : entity'nin sorumluluğu sadece veriyi taşımak olmamalı. Aynı zamanda kendi davranışlarını da içermelidir.
    public class Category : AuditableEntity
    {
        public string Name { get; private set; }

        private Category()
        {

        }

        public Category(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Kategori adı boş olamaz") : name;
        }


        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new DomainException("Kategori adı boş olamaz.");
            Name = newName;
            TouchUpdated();
        }
    }
}
