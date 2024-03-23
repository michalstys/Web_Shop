using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;

namespace Web_Shop.Application.Features.Commands
{
    public class UpdateCustomerCommand : AddUpdateCustomerDTO, IRequest<GetSingleCustomerDTO>
    {
        public ulong CustomerId;

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, GetSingleCustomerDTO>
        {
            private readonly ICustomerService _customerService;

            public UpdateCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<GetSingleCustomerDTO> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                var result = await _customerService.UpdateExistingCustomerAsync(request, request.CustomerId);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }

                return result.entity!.MapGetSingleCustomerDTO();
            }
        }
    }
}
