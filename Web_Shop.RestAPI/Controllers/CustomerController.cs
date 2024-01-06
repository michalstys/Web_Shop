using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Application.Mappings;
using Sieve.Models;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetCustomerById")]
        public async Task<ActionResult<GetSingleCustomerDTO>> GetCustomer(ulong id)
        {
            var result = await _customerService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO());
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCustomers")]
        public async Task<IActionResult> GetCustomers([FromQuery] SieveModel paginationParams)
        {
            var result = await _customerService.SearchCustomersAsync(paginationParams);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }
    }
}
