using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Results
{
    public sealed record Error(string Code, string? Message = null)
    {
        public static readonly Error None = new(string.Empty, null);

        public static Error Validation(string code, string message)
            => new(code, message);

        public static Error Business(string code)
            => new(code, null);

        public static Error Business(string code, string message)
            => new(code, message);
    }
}
