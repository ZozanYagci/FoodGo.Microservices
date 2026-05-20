using FoodGo.CatalogService.Api.ProblemDetails;
using FoodGo.CatalogService.Application.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodGo.CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            var error = result.Errors.First();

            return error.Type switch
            {
                ErrorType.Validation =>
                BadRequest(
                    new ValidationErrorProblemDetails(
                    new Dictionary<string, string[]>
                    {
                        {error.Code, new[] { error.Message } }
                        })),

                ErrorType.NotFound =>
                NotFound(
                    new BusinessProblemDetails(
                        error.Code,
                        error.Message)),


                ErrorType.Business =>
                Conflict(
                    new BusinessProblemDetails(
                        error.Code,
                        error.Message)),

                _ =>
                StatusCode(StatusCodes.Status500InternalServerError,
                new InternalServerErrorProblemDetails())
            };
        }
    }
}
