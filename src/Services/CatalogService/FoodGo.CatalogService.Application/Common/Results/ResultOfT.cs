using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Results
{
    public class Result<T> : Result
    {

        private readonly T? _value;

        protected Result(T value)
            : base(true, Array.Empty<Error>())
        {
            _value = value;
        }

        protected Result(IEnumerable<Error> errors)
            : base(false, errors.ToArray())
        {
            _value = default;
        }

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Failure result has no value");

        public static Result<T> Success(T value)
            => new(value);

        public static Result<T> Failure(IEnumerable<Error> errors)
            => new(errors);

        public static Result<T> Failure(Error error)
            => new(new[] { error });
    }
}
