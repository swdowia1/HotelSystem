using ddd.Application;
using ddd.VM;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ddd.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var result = await _mediator.Send(new ListOrdersQuery());
            return Ok(result);
        }
    }
}
