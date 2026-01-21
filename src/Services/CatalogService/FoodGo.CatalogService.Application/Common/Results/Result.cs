using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Application.Common.Results
{
    public class Result
    {
        protected Result(bool isSuccess, IReadOnlyCollection<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors.ToArray();
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public IReadOnlyCollection<Error> Errors { get; }

        public static Result Success()
            => new(true, Array.Empty<Error>());

        public static Result Failure(IEnumerable<Error> errors)
            => new(false, errors.ToArray());

        public static Result Failure(Error error)
            => new(false, new[] { error });
    }
}
