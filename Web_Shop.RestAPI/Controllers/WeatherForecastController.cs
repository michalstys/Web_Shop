using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WWSI_Shop.Persistence.MySQL.Context;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly WwsishopContext _dbContext;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, WwsishopContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetCustomerById")]
        public async Task<IActionResult> GetCustomer(ulong id)
        {
            return Ok(await _dbContext.Customers.FirstOrDefaultAsync(customer => customer.IdCustomer == id));
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _dbContext.Customers.ToListAsync());
        }
    }
}
