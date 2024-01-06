using Sieve.Models;
using System.Net;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Helpers.PagedList;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Services.Interfaces
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<(bool IsSuccess, IPagedList<Customer, GetSingleCustomerDTO>? entityList, HttpStatusCode StatusCode, string ErrorMessage)> SearchCustomersAsync(SieveModel paginationParams);
    }
}
