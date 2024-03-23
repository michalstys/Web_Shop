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
    public class GetSingleCustomerQuery : IRequest<GetSingleCustomerDTO>
    {
        public ulong CustomerId { get; set; }

        public class GetAllCustomersQueryHandler : IRequestHandler<GetSingleCustomerQuery, GetSingleCustomerDTO>
        {
            private readonly ICustomerService _customerService;

            public GetAllCustomersQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<GetSingleCustomerDTO> Handle(GetSingleCustomerQuery request, CancellationToken cancellationToken)
            {
                var result = await _customerService.GetByIdAsync(request.CustomerId);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }

                return result.entity!.MapGetSingleCustomerDTO();
            }
        }
    }
}
