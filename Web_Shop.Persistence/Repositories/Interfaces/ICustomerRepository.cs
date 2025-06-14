using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Persistence.Repositories.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<List<Customer>> GetCustomers();
        Task<bool> EmailExistsAsync(string email);
        Task<bool> IsEmailEditAllowedAsync(string email, ulong id);
        Task<Customer?> GetByEmailAsync(string email);
    }
}
