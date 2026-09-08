using FoodGo.CatalogService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.Entities
{
    // DDD : entity'nin sorumluluğu sadece veriyi taşımak olmamalı. Aynı zamanda kendi davranışlarını da içermelidir.
    public class Category : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; }

        private Category()
        {

        }

        public Category(string name)
        {
            Name = ValidateName(name);
        }


        public void Rename(string newName)
        {
            newName = ValidateName(newName);

            if (Name == newName)
                return;
            Name = newName;
            TouchUpdated();
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category.Name.Empty");

            return name.Trim();
        }
    }
}
