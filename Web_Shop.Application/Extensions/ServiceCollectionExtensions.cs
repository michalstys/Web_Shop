using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web_Shop.Application.Services;
using Web_Shop.Application.Services.Interfaces;

namespace Web_Shop.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
        }
    }
}
