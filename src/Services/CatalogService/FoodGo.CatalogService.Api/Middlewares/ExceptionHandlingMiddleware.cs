using FluentValidation;
using FoodGo.CatalogService.Domain.SeedWork;
using System.Net;
using System.Text.Json;

namespace FoodGo.CatalogService.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug("Validation failed for request {Path}", context.Request.Path);
                await HandleValidationException(context, ex);
            }
            catch (DomainException ex)
            {
                await HandleDomainException(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleInternalServerError(context, ex);
            }
        }

        private static async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new
            {
                title = "Validation Error",
                status = 400,
                errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }

        private static async Task HandleDomainException(HttpContext context, DomainException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";

            var response = new
            {
                title = "Business Rule Violation",
                status = 409,
                detail = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }

        private static async Task HandleInternalServerError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                title = "Internal Server Error",
                status = 500,
                detail = "Beklenmeyen bir hata oluştu."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }


        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
    }

}
