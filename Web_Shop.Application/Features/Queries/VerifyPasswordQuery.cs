using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;

namespace Web_Shop.Application.Features.Queries
{
    public class VerifyPasswordQuery : IRequest<GetSingleCustomerDTO>
    {
        public string Email = null!;
        public string Password = null!;

        public class VerifyPasswordQueryHandler: IRequestHandler<VerifyPasswordQuery, GetSingleCustomerDTO>
        {
            private readonly ICustomerService _customerService;

            public VerifyPasswordQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<GetSingleCustomerDTO> Handle(VerifyPasswordQuery request, CancellationToken cancellationToken)
            {
                var result = await _customerService.VerifyPasswordByEmail(email: request.Email, password: request.Password);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }

                return result.entity!.MapGetSingleCustomerDTO();
            }
        }
    }
}
