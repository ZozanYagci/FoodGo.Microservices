using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Results
{
    public sealed record Error(string Code, string? Message, ErrorType Type)
    {
        public static readonly Error None = new(string.Empty, null, ErrorType.Validation);

        public static Error Validation(string code, string message)
            => new(code, message, ErrorType.Validation);

        public static Error Business(string code)
            => new(code, null, ErrorType.Business);

        public static Error Business(string code, string message)
            => new(code, message, ErrorType.Business);

        public static Error NotFound(string code, string message)
            => new Error(code, message, ErrorType.NotFound);
    }
}
