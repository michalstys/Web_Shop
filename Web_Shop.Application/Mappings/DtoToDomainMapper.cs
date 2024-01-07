using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using WWSI_Shop.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop.Application.Mappings
{
    public static class DtoToDomainMapper
    {
        public static Customer MapCustomer(this AddUpdateCustomerDTO dtoCustomer)
        {
            if (dtoCustomer == null)
                throw new ArgumentNullException(nameof(dtoCustomer));

            Customer domainCustomer = new()
            {
                Name = dtoCustomer.Name,
                Surname = dtoCustomer.Surname,
                Email = dtoCustomer.Email,
                BirthDate = dtoCustomer.BirthDate,
                PasswordHash = BC.HashPassword(dtoCustomer.Password)
            };

            return domainCustomer;
        }
    }
}
