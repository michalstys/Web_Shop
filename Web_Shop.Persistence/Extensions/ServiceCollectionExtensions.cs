using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web_Shop.Persistence.MySQL.Extensions;
using Web_Shop.Persistence.Repositories;
using Web_Shop.Persistence.Repositories.Interfaces;

namespace Web_Shop.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMySQLDbContext(configuration);

            services.AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));
        }
    }
}
