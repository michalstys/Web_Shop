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
using BC = BCrypt.Net.BCrypt;

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

        public async Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewCustomerAsync(AddUpdateCustomerDTO dto)
        {
            try
            {
                if (await _unitOfWork.CustomerRepository.EmailExistsAsync(dto.Email))
                {
                    return (false, default(Customer), HttpStatusCode.BadRequest, "Email: " + dto.Email + " already registered.");
                }

                var newEntity = dto.MapCustomer();
                //newEntity.CreatedAt = DateTime.UtcNow;
                //newEntity.UpdatedAt = newEntity.CreatedAt;

                var result = await AddAndSaveAsync(newEntity);
                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingCustomerAsync(AddUpdateCustomerDTO dto, ulong id)
        {
            try
            {
                var existingEntityResult = await WithoutTracking().GetByIdAsync(id);

                if (!existingEntityResult.IsSuccess)
                {
                    return existingEntityResult;
                }

                if (!await _unitOfWork.CustomerRepository.IsEmailEditAllowedAsync(dto.Email, id))
                {
                    return (false, default(Customer), HttpStatusCode.BadRequest, "Email: " + dto.Email + " already registered.");
                }

                var domainEntity = dto.MapCustomer();

                domainEntity.IdCustomer = id;
                if (!dto.IsPasswordUpdate)
                {
                    domainEntity.PasswordHash = existingEntityResult!.entity!.PasswordHash;
                }

                //domainEntity.CreatedAt = existingEntity.CreatedAt;
                //domainEntity.UpdatedAt = DateTime.UtcNow;
                //domainCustomer.UpdatedAt = DateTime.UtcNow.ConvertFromUtc(TimeZones.CentralEuropeanTimeZone);
                return await UpdateAndSaveAsync(domainEntity, id);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        /*
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
        */

        public async Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> VerifyPasswordByEmail(string email, string password)
        {
            try
            {
                var existingEntity = await _unitOfWork.CustomerRepository.GetByEmailAsync(email);

                if (existingEntity == null || !BC.Verify(password, existingEntity.PasswordHash))
                {
                    return (false, default(Customer), HttpStatusCode.Unauthorized, "Invalid email or password.");
                }

                return (true, existingEntity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }
    }
}
