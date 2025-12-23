using FluentValidation;
using FoodGo.CatalogService.Api.ProblemDetails;
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

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

                var problemsDetails = new HttpValidationProblemDetails(errors);

                await WriteProblemDetailsAsync(context, problemsDetails);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning("Business rule violation : {Message}", ex.Message);

                var problemDetails = new BusinessProblemDetails(ex.Message);

                await WriteProblemDetailsAsync(context, problemDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                var problemDetails = new InternalServerErrorProblemDetails();

                await WriteProblemDetailsAsync(context, problemDetails);
            }
        }

        private static async Task WriteProblemDetailsAsync(HttpContext context, Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails)
        {
            context.Response.StatusCode = problemDetails.Status!.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);

        }
    }

}
