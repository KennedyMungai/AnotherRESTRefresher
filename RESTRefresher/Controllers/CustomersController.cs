using Microsoft.AspNetCore.Mvc;
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
}