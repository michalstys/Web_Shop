using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.Helpers.PagedList;

namespace Web_Shop.Application.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        IBaseService<T> WithTracking();
        IBaseService<T> WithoutTracking();
        Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> GetByIdAsync(params object?[]? id);
        Task<(bool IsSuccess, IPagedList<TOut>? entityList, HttpStatusCode StatusCode, string ErrorMessage)> SearchAsync<TOut>(SieveModel paginationParams, Func<T, TOut> formatterCallback);
        Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> AddAsync(T entity);
        Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> AddAndSaveAsync(T entity);
        Task<T> UpdateAsync(T entity, params object?[]? id);
        Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateAndSaveAsync(T entity, params object?[]? id);
        Task DeleteAsync(params object?[]? id);
        Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> DeleteAndSaveAsync(params object?[]? id);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
