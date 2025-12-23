namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public sealed class AuthorizationProblemDetails : BaseProblemDetails
    {
        public AuthorizationProblemDetails(string detail)
            : base("Unauthorized", StatusCodes.Status401Unauthorized)
        {
            Detail = detail;
        }
    }
}
