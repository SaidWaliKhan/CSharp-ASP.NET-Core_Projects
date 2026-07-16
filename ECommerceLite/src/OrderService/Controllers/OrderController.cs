using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Command;

namespace OrderService.Controllers;

public record CreateOrderRequest(Guid ProductId, int Quantity, decimal TotalPrice);

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        // Forward the same token the client used, so OrderService can call
        // ProductService "on behalf of" the original caller.
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        var command = new CreateOrderCommand(request.ProductId, request.Quantity, request.TotalPrice, token);
        var orderId = await _mediator.Send(command);

        return Ok(new { orderId });
    }
}