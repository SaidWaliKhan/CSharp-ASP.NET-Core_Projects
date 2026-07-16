using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Infrastructure.Persistence;
using ProductCatalog.Domain.Entities;
using MediatR;
using ProductCatalog.Application.Products.Commands.CreateProduct;
using ProductCatalog.Application.Products.Queries.GetProducts;
using ProductCatalog.Application.Common.Results;

namespace ProductCatalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{

    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Result> GetAll()
    {
        var products =
            await _mediator.Send(new GetProductsQuery());

        return Result.Success();
    }




    [HttpPost]
    public async Task<IActionResult> Create(
    CreateProductCommand command)
    {
        var product = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(command),
            new { id = product.Id },
            product);
    }


}