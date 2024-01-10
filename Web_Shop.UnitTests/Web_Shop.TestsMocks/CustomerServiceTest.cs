using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using Sieve.Services;
using Web_Shop.Application.CustomQueries;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Mappings.PropertiesMappings;
using Web_Shop.Application.Services;
using Web_Shop.Persistence.Repositories.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.UnitTests.Common.Sieve;
using WWSI_Shop.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop.TestsMocks
{
    public class CustomerServiceTest
    {
        private readonly Mock<ILogger<Customer>> _loggerMock;

        private readonly Mock<ApplicationSieveProcessor> _processorMock;
        private readonly Mock<SieveOptionsAccessor> _optionsAccessorMock;

        public CustomerServiceTest()
        {
            _loggerMock = new Mock<ILogger<Customer>>();

            _optionsAccessorMock = new Mock<SieveOptionsAccessor>();

            _processorMock = new Mock<ApplicationSieveProcessor>(_optionsAccessorMock.Object,
                new Mock<SieveCustomSortMethods>().Object,
                new Mock<SieveCustomFilterMethods>().Object);
        }

        public static Mock<ICustomerRepository> GetICustomerRepositoryMock()
        {
            var mock = new Mock<ICustomerRepository>();

            mock.Setup(m => m.GetByEmailAsync(It.IsAny<string>())).Returns((string email) => Task.FromResult(new Customer { IdCustomer = 2, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@o2.pl", PasswordHash = BC.HashPassword("Test222") }));

            return mock;
        }

        public static Mock<IUnitOfWork> GetIUnitOfWorkMock()
        {
            var mock = new Mock<IUnitOfWork>();

            mock.Setup(m => m.CustomerRepository).Returns(() => GetICustomerRepositoryMock().Object);

            return mock;
        }

        [Fact]
        public async Task VerifyPasswordByEmailTestPositive()
        {
            var customerService = new CustomerService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, GetIUnitOfWorkMock().Object);

            var verifyResult = await customerService.VerifyPasswordByEmail("jan.kowalski@o2.pl", "Test222");

            Assert.True(verifyResult.IsSuccess);
            Assert.Equal("jan.kowalski@o2.pl", verifyResult.entity!.Email);
        }

        [Fact]
        public async Task VerifyPasswordByEmailTestNegative()
        {
            var customerService = new CustomerService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, GetIUnitOfWorkMock().Object);

            var verifyResult = await customerService.VerifyPasswordByEmail("jan.kowalski@o2.pl", "Test211");

            Assert.False(verifyResult.IsSuccess);
        }
    }
}