using System.Diagnostics;
using System;
using System.Net;
using System.Xml.Linq;
using Web_Shop.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Web_Shop.Persistence.UOW.Interfaces;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using Microsoft.EntityFrameworkCore;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Helpers.PagedList;
using WWSI_Shop.Persistence.MySQL.Model;
using Web_Shop.Application.Extensions;

namespace Web_Shop.Application.Services
{

    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly ILogger<T> _logger;

        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ISieveProcessor _sieveProcessor;
        protected readonly IOptions<SieveOptions> _sieveOptions;

        private bool _tracking = true;

        public BaseService(ILogger<T> logger,
                           ISieveProcessor sieveProcessor,
                           IOptions<SieveOptions> sieveOptions,
                           IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _sieveProcessor = sieveProcessor;
            _sieveOptions = sieveOptions;
        }

        public IBaseService<T> WithTracking()
        {
            _tracking = true;
            return this;
        }

        public IBaseService<T> WithoutTracking()
        {
            _tracking = false;
            return this;
        }

        public async Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> GetByIdAsync(params object?[]? id)
        {
            try
            {
                var result = _tracking
                    ? await _unitOfWork.Repository<T>().GetByIdAsync(id)
                    : await _unitOfWork.Repository<T>().WithoutTracking().GetByIdAsync(id);

                if (result == null)
                {
                    return (false, default(T), HttpStatusCode.NotFound, "Object not found in database.");
                }

                return (true, result, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IPagedList<TOut>? entityList, HttpStatusCode StatusCode, string ErrorMessage)> SearchAsync<TOut>(SieveModel paginationParams, Func<T, TOut> formatterCallback)
        {
            try
            {
                var query = _unitOfWork.Repository<T>().Entities.AsNoTracking();

                var result = await query.ToPagedListAsync(_sieveProcessor,
                                                          _sieveOptions,
                                                          paginationParams,
                                                          formatterCallback);

                return (true, result, HttpStatusCode.OK, String.Empty);
            }
            catch (Exception ex)
            {
                var error = LogError(ex.Message);

                return (false, default, error.StatusCode, error.ErrorMessage);
            }
        }

        public async Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> AddAsync(T entity)
        {
            try
            {
                var result = await _unitOfWork.Repository<T>().AddAsync(entity);

                return (true, result, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> AddAndSaveAsync(T entity)
        {
            try
            {
                var result = await _unitOfWork.Repository<T>().AddAsync(entity);
                await SaveChangesAsync(CancellationToken.None);

                return (true, result, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<T> UpdateAsync(T entity, params object?[]? id)
        {
            return await _unitOfWork.Repository<T>().UpdateAsync(entity, id);
        }

        public async Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateAndSaveAsync(T entity, params object?[]? id)
        {
            if (id == null)
            {
                return (false, default(T), HttpStatusCode.BadRequest, "Object ID cannot be null or empty.");
            }

            try
            {
                if (!await _unitOfWork.Repository<T>().Exists(id))
                {
                    return (false, default(T), HttpStatusCode.NotFound, "Object " + typeof(T) + " with ID " + id[0] + " does not exists in database.");
                }

                var result = await _unitOfWork.Repository<T>().UpdateAsync(entity, id);
                await SaveChangesAsync(CancellationToken.None);

                return (true, result, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task DeleteAsync(params object?[]? id)
        {
            var entity = await _unitOfWork.Repository<T>().GetByIdAsync(id);

            await _unitOfWork.Repository<T>().DeleteAsync(entity);

            return;
        }

        public async Task<(bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage)> DeleteAndSaveAsync(params object?[]? id)
        {
            if (id == null)
            {
                return (false, default(T), HttpStatusCode.BadRequest, "Object ID cannot be null or empty.");
            }

            try
            {
                if (!await _unitOfWork.Repository<T>().Exists(id))
                {
                    return (false, default(T), HttpStatusCode.NotFound, "Object " + typeof(T) + " with ID " + id[0] + " does not exists in database.");
                }

                await DeleteAsync(id);
                await SaveChangesAsync(CancellationToken.None);

                return (true, default(T), HttpStatusCode.NoContent, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        protected (bool IsSuccess, T? entity, HttpStatusCode StatusCode, string ErrorMessage) LogError(string errorMessage)
        {
            var traceId = Activity.Current?.TraceId;
            var spanId = Activity.Current?.SpanId;

            _logger.LogInformation("Exception: " + errorMessage + " TraceID: " + traceId + "-" + spanId);

            return (false, default(T), HttpStatusCode.InternalServerError, "Internal server error.");
        }
    }
}
