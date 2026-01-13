using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.SeedWork.DomainErrors
{
    public static class RestaurantErrors
    {
        public const string NameCannotBeEmpty = "Restaurant.Name.CannotBeEmpty";
        public const string AddressCannotBeNull = "Restaurant.Address.CannotBeNull";
        public const string CategoryAlreadyExist = "Restaurant.Category.AlreadyExist";
        public const string RestaurantInactive = "Restaurant.Inactive";
        public const string NameAlreadyExists = "Restaurant.Name.AlreadyExists";
        public const string CategoryLimitExceeded = "Restaurant.Category.LimitExceeded";
        public const string RestaurantNotFound = "Restaurant.NotFound";

    }
}
