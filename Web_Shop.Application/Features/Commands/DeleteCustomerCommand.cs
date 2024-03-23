using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.Services.Interfaces;

namespace Web_Shop.Application.Features.Commands
{
    public class DeleteCustomerCommand : IRequest
    {
        public ulong CustomerId;

        public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
        {
            private readonly ICustomerService _customerService;

            public DeleteCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
            {
                var result = await _customerService.DeleteAndSaveAsync(request.CustomerId);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.ErrorMessage);
                }
            }
        }
    }
}
