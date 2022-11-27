using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> OrderList(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(query);
            if (orders.Count <= 0 || orders == null)
                return BadRequest("This user doesn't have any orders");

            return Ok(orders);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletOrder(int Id)
        {
            var command = new DeleteOrderCommand { Id = Id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutOrder(CheckOutOrderCommand checkOutCommand)
        {
            var result = await _mediator.Send(checkOutCommand);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand updateCommand)
        {
            await _mediator.Send(updateCommand);
            return NoContent();
        }

        

    }
}
