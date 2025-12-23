namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public sealed class ValidationProblemDetails : BaseProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationProblemDetails(IDictionary<string, string[]> errors)
            : base("Validation Error", StatusCodes.Status400BadRequest)
        {
            Errors = errors;
        }

    }
}
