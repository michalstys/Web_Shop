using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Application.Mappings;
using Sieve.Models;
using Web_Shop.Application.Helpers.PagedList;
using WWSI_Shop.Persistence.MySQL.Model;
using MediatR;
using Web_Shop.Application.Features.Queries;
using System.Net;
using Web_Shop.Application.Features.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetCustomerById")]
        public async Task<ActionResult<GetSingleCustomerDTO>> GetCustomer(ulong id)
        {
            GetSingleCustomerDTO customer;

            try
            {
                customer = await _mediator.Send(new GetSingleCustomerQuery() { CustomerId = id });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int?)HttpStatusCode.InternalServerError, title: "Read error.", detail: ex.Message);
            }

            return StatusCode((int)HttpStatusCode.OK, customer);
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCustomers")]
        public async Task<ActionResult<IPagedList<GetSingleCustomerDTO>>> GetCustomers([FromQuery] SieveModel paginationParams)
        {
            IPagedList<GetSingleCustomerDTO> customersList;

            try
            {
                customersList = await _mediator.Send(new GetAllCustomersQuery() { paginationParams = paginationParams });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int?)HttpStatusCode.InternalServerError, title: "Read error.", detail: ex.Message);
            }

            return Ok(customersList);
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddCustomer")]
        public async Task<ActionResult<GetSingleCustomerDTO>> AddCustomer([FromBody] AddCustomerCommand command)
        {
            GetSingleCustomerDTO customer;

            try
            {
                customer = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, title: "Add error.", detail: ex.Message);
            }

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.IdCustomer }, customer);
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation(OperationId = "UpdateCustomer")]
        public async Task<ActionResult<GetSingleCustomerDTO>> UpdateCustomer(ulong id, [FromBody] UpdateCustomerCommand command)
        {
            GetSingleCustomerDTO customer;

            command.CustomerId = id;
            
            try
            {
                customer = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, title: "Update error.", detail: ex.Message);
            }

            return StatusCode((int)HttpStatusCode.OK, customer);
        }

        [HttpGet("verifyPassword/{email}/{password}")]
        [SwaggerOperation(OperationId = "VerifyPasswordByEmail")]
        public async Task<ActionResult<GetSingleCustomerDTO>> VerifyPasswordByEmail(string email, string password)
        {
            GetSingleCustomerDTO customer;

            try
            {
                customer = await _mediator.Send(new VerifyPasswordQuery() { Email = email, Password = password });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int?)HttpStatusCode.InternalServerError, title: "Read error.", detail: ex.Message);
            }

            return StatusCode((int)HttpStatusCode.OK, customer);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(ulong id)
        {
            try
            {
                await _mediator.Send(new DeleteCustomerCommand() { CustomerId = id });
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, title: "Delete error.", detail: ex.Message);
            }

            return NoContent();
        }
    }
}
