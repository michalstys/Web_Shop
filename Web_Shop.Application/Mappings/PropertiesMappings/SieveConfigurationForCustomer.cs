using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Mappings.PropertiesMappings
{
    public class SieveConfigurationForCustomer : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Customer>(p => p.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Customer>(p => p.Surname)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Customer>(p => p.Email)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Customer>(p => p.BirthDate)
                 .CanSort()
                 .CanFilter();
        }
    }
}
