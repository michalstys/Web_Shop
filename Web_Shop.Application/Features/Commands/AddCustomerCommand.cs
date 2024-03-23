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
    public class AddCustomerCommand : AddUpdateCustomerDTO, IRequest<GetSingleCustomerDTO>
    {
        public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, GetSingleCustomerDTO>
        {
            private readonly ICustomerService _customerService;

            public AddCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<GetSingleCustomerDTO> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
            {
                var result = await _customerService.CreateNewCustomerAsync(request);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }

                return result.entity!.MapGetSingleCustomerDTO();
            }
        }
    }
}
