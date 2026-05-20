using FoodGo.CatalogService.Application.Features.Restaurants.Commands.CreateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.DeleteRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Commands.UpdateRestaurant;
using FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetAllRestaurants;
using FoodGo.CatalogService.Application.Features.Restaurants.Queries.GetRestaurantById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// deneme - CI pipeline test 

namespace FoodGo.CatalogService.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : BaseApiController
    {
        private readonly IMediator _mediator;

        public RestaurantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return HandleResult(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRestaurantCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await _mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteRestaurantCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetRestaurantByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetRestaurantsQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
