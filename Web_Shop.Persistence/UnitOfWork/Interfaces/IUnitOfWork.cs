using Web_Shop.Persistence.Repositories.Interfaces;

namespace Web_Shop.Persistence.UOW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }

        IProductRepository ProductRepository { get; }

        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();
    }
}
