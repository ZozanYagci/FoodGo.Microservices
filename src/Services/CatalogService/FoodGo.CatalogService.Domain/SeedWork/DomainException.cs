using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.SeedWork
{
    public class DomainException : Exception
    {

        public string ErrorCode { get; }
        public DomainException(string errorCode) : base(errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
