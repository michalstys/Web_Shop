using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Mappings
{
    public static class DomainToDtoMapper
    {
        public static GetSingleCustomerDTO MapGetSingleCustomerDTO(this Customer domainCustomer)
        {
            if (domainCustomer == null)
                throw new ArgumentNullException(nameof(domainCustomer));

            GetSingleCustomerDTO getSingleCustomerDTO = new()
            {
                IdCustomer = domainCustomer.IdCustomer,
                Name = domainCustomer.Name,
                Surname = domainCustomer.Surname,
                Email = domainCustomer.Email,
                BirthDate = domainCustomer.BirthDate,
            };

            return getSingleCustomerDTO;
        }

        public static GetSingleProductDTO MapGetSingleProductDTO(this Product domainProduct)
        {
            if (domainProduct == null)
                throw new ArgumentNullException(nameof(domainProduct));

            GetSingleProductDTO getSingleProductDTO = new()
            {
                IdProduct = domainProduct.IdProduct,
                Name = domainProduct.Name,
                Description = domainProduct.Description,
                Price = domainProduct.Price,
                Sku = domainProduct.Sku,
            };

            return getSingleProductDTO;
        }
    }
}
