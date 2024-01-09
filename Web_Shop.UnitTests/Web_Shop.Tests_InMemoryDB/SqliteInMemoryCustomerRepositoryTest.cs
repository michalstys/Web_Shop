using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Persistence.Repositories;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop.Tests_InMemoryDB
{
    public class SqliteInMemoryCustomerRepositoryTest
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<WwsishopContext> _contextOptions;

        #region ConstructorAndDispose
        public SqliteInMemoryCustomerRepositoryTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<WwsishopContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new WwsishopContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT Name
FROM Customer;";
                viewCommand.ExecuteNonQuery();
            }
            context.AddRange(
                new Customer { IdCustomer = 1, Name = "Michał", Surname = "Styś", Email = "michal.stys@gmail.com", PasswordHash = BC.HashPassword("Test111") },
                new Customer { IdCustomer = 2, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@o2.pl", PasswordHash = BC.HashPassword("Test222") });
            context.SaveChanges();

        }

        WwsishopContext CreateContext() => new WwsishopContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        [Fact]
        public async Task EmailExistsAsyncPositive()
        {
            using var context = CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailExists = await customerRepository.EmailExistsAsync("michal.stys@gmail.com");

            Assert.True(emailExists);
        }

        [Fact]
        public async Task EmailExistsAsyncNegative()
        {
            using var context = CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailExists = await customerRepository.EmailExistsAsync("jan.nowak@gmail.com");

            Assert.False(emailExists);
        }

        [Fact]
        public async Task IsEmailEditAllowedAsyncPositive()
        {
            using var context = CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailEditAllowed = await customerRepository.IsEmailEditAllowedAsync("michal.stys@wp.pl", 1);

            Assert.True(emailEditAllowed);
        }

        [Fact]
        public async Task IsEmailEditAllowedAsyncNegative()
        {
            using var context = CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailEditAllowed = await customerRepository.IsEmailEditAllowedAsync("jan.kowalski@o2.pl", 1);

            Assert.False(emailEditAllowed);
        }

        [Fact]
        public async Task GetByEmailAsync()
        {
            using var context = CreateContext();

            var customerRepository = new CustomerRepository(context);

            var customer = await customerRepository.GetByEmailAsync("michal.stys@gmail.com");

            Assert.Equal("Michał", customer.Name);
            Assert.Equal("Styś", customer.Surname);
        }
    }
}
