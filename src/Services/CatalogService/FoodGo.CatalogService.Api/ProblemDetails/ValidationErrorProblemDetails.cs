namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public sealed class ValidationErrorProblemDetails : BaseProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationErrorProblemDetails(IDictionary<string, string[]> errors)
            : base("Validation Error", StatusCodes.Status400BadRequest)
        {
            Errors = errors;
        }

    }
}
