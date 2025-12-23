namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public sealed class InternalServerErrorProblemDetails : BaseProblemDetails
    {
        public InternalServerErrorProblemDetails()
            : base("Internal Server Error", StatusCodes.Status500InternalServerError)
        {
            Detail = "An unexpected error occurred.";
        }
    }
}
