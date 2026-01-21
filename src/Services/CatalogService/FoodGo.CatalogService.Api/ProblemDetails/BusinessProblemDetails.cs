namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public sealed class BusinessProblemDetails : BaseProblemDetails
    {

        public string ErrorCode { get; }
        public BusinessProblemDetails(string errorCode, string? message)
            : base("Business Rule Violation", StatusCodes.Status409Conflict)
        {
            ErrorCode = errorCode;
            Detail = message;
        }
    }
}
