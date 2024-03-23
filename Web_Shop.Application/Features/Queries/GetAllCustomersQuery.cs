using MediatR;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Helpers.PagedList;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;

namespace Web_Shop.Application.Features.Queries
{
    public class GetAllCustomersQuery : IRequest<IPagedList<GetSingleCustomerDTO>>
    {
        public SieveModel paginationParams { get; set; } = null!;

        public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IPagedList<GetSingleCustomerDTO>>
        {
            private readonly ICustomerService _customerService;

            public GetAllCustomersQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<IPagedList<GetSingleCustomerDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
            {
                var result = await _customerService.SearchAsync(request.paginationParams, 
                                                                resultEntity => DomainToDtoMapper.MapGetSingleCustomerDTO(resultEntity));

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }

                return result!.entityList!;
            }
        }
    }
}
