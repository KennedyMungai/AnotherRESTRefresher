using Microsoft.AspNetCore.Mvc;
using RESTRefresher.Models;
using RESTRefresher.Repositories;

namespace RESTRefresher.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository repo;

    public CustomersController(ICustomerRepository repo)
    {
            this.repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Customer>> GetCustomers(string? country)
    {
        if(string.IsNullOrWhiteSpace(country))
        {
            return await repo.RetrieveAllAsync();
        }
        else
        {
            return(await repo.RetrieveAllAsync())
                    .Where(customer => customer.Country == country);
        }
    }

    [HttpGet("{id}", Name = nameof(GetCustomer))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(string id)
    {
        Customer? c = await repo.RetrieveAsync(id);

        if (c is null)
        {
            return NotFound();
        }

        return Ok(c);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Customer c)
    {
        if (c is null)
        {
            return BadRequest();
        }

        Customer? addedCustomer = await repo.CreateAsync(c);

        if(addedCustomer is null)
        {
            return BadRequest();
        }
        else
        {
            return CreatedAtRoute(
                routeName: nameof(GetCustomer),
                routeValues: new {id = addedCustomer.CustomerId.ToLower()},
                value: addedCustomer
            );
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] Customer c)
    {
        id = id.ToUpper();
        c.CustomerId = c.CustomerId.ToUpper();

        if (c is null or c.CustomerId != id)
        {
            return BadRequest();
        }

        Customer? existing = await repo.RetrieveAsync(id);

        if (existing is null)
        {
            return NotFound();
        }

        await repo.UpdateAsync(id, c);

        return new NoContentResult();
    }
}