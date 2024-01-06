using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Extensions;
using Web_Shop.Application.Helpers.PagedList;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(ILogger<Customer> logger,
                               ISieveProcessor sieveProcessor,
                               IOptions<SieveOptions> sieveOptions,
                               IUnitOfWork unitOfWork)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {

        }

        public async Task<(bool IsSuccess, IPagedList<Customer, GetSingleCustomerDTO>? entityList, HttpStatusCode StatusCode, string ErrorMessage)> SearchCustomersAsync(SieveModel paginationParams)
        {
            try
            {
                var query = _unitOfWork.CustomerRepository.Entities.AsNoTracking();

                var result = await query.ToPagedListAsync(_sieveProcessor,
                                                          _sieveOptions,
                                                          paginationParams,
                                                          formatterCallback => DomainToDtoMapper.MapGetSingleCustomerDTO(formatterCallback));

                return (true, result, HttpStatusCode.OK, String.Empty);
            }
            catch (Exception ex)
            {
                var error = LogError(ex.Message);

                return (false, default, error.StatusCode, error.ErrorMessage);
            }
        }
    }
}
