using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
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

namespace Web_Shop.Tests_InMemoryDB
{
    public class SqliteInMemoryCustomerRepositoryTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;
        public SqliteInMemoryCustomerRepositoryTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();
        }
        public void Dispose()
        {
            _databaseFixture.Dispose();
        }

        [Fact]
        public async Task CustomerRepository_EmailExistsAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailExists = await customerRepository.EmailExistsAsync("michal.stys@gmail.com");

            Assert.True(emailExists);
        }

        [Fact]
        public async Task CustomerRepository_EmailExistsAsync_ReturnsFalse()
        {
            using var context = _databaseFixture.CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailExists = await customerRepository.EmailExistsAsync("jan.nowak@gmail.com");

            Assert.False(emailExists);
        }

        [Fact]
        public async Task CustomerRepository_IsEmailEditAllowedAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailEditAllowed = await customerRepository.IsEmailEditAllowedAsync("michal.stys@wp.pl", 1);

            Assert.True(emailEditAllowed);
        }

        [Fact]
        public async Task CustomerRepository_IsEmailEditAllowedAsync_ReturnsFalse()
        {
            using var context = _databaseFixture.CreateContext();

            var customerRepository = new CustomerRepository(context);

            var emailEditAllowed = await customerRepository.IsEmailEditAllowedAsync("jan.kowalski@o2.pl", 1);

            Assert.False(emailEditAllowed);
        }

        [Fact]
        public async Task CustomerRepository_GetByEmailAsync_Exists()
        {
            using var context = _databaseFixture.CreateContext();

            var customerRepository = new CustomerRepository(context);

            var customer = await customerRepository.GetByEmailAsync("michal.stys@gmail.com");

            Assert.Equal("Michał", customer.Name);
            Assert.Equal("Styś", customer.Surname);
        }

        [Fact]
        public async Task CustomerRepository_GetByEmailAsync_NotExist()
        {
            using var context = _databaseFixture.CreateContext();

            var customerRepository = new CustomerRepository(context);

            var customer = await customerRepository.GetByEmailAsync("jan.nowak@gmail.com");

            Assert.Null(customer);
        }
    }
}
