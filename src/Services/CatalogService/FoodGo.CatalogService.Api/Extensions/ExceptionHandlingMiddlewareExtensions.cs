using FoodGo.CatalogService.Api.Middlewares;

namespace FoodGo.CatalogService.Api.Extensions
{
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
