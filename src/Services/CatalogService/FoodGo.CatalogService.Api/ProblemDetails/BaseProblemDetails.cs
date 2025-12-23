namespace FoodGo.CatalogService.Api.ProblemDetails
{
    public abstract class BaseProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        protected BaseProblemDetails(string title, int status)
        {
            Title = title;
            Status = status;
            Type = $"https://httpstatues.com/{status}";
            Instance = Guid.NewGuid().ToString();
        }
    }
}
