namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public sealed class BusinessProblemDetails : BaseProblemDetails
    {
        public BusinessProblemDetails(string detail)
            : base("Business Rule Violation", StatusCodes.Status409Conflict)
        {
            Detail = detail;
        }
    }
}
