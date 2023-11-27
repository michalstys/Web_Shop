using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WWSI_Shop.Persistence.MySQL.Context;

namespace Web_Shop.Persistence.MySQL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMySQLDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = (configuration.GetConnectionString("WwsiShopDatabase"))
                    ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<WwsishopContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }
    }
}
