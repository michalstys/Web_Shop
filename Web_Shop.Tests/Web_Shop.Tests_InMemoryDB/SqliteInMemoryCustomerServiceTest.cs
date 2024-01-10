using Castle.Core.Logging;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.CustomQueries;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Mappings.PropertiesMappings;
using Web_Shop.Application.Services;
using Web_Shop.Persistence.Repositories;
using Web_Shop.Persistence.Repositories.Interfaces;
using Web_Shop.Persistence.UOW;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Tests.Common.Sieve;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Tests_InMemoryDB
{
    public class SqliteInMemoryCustomerServiceTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;

        private readonly Mock<ILogger<Customer>> _loggerMock;

        private readonly SieveProcessor _processor;
        private readonly SieveOptionsAccessor _optionsAccessor;

        public SqliteInMemoryCustomerServiceTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();

            _loggerMock = new Mock<ILogger<Customer>>();

            _optionsAccessor = new SieveOptionsAccessor();

            _processor = new ApplicationSieveProcessor(_optionsAccessor,
                new SieveCustomSortMethods(),
                new SieveCustomFilterMethods());
        }
        public void Dispose()
        {
            _databaseFixture.Dispose();
        }

        [Fact]
        public async Task CustomerService_CreateNewCustomerAsync_ReturnsTrue()
        {
            {
                using var context = _databaseFixture.CreateContext();

                var unitOfWork = new UnitOfWork(context);

                var customerService = new CustomerService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

                var addUpdateCustomerDTO = new AddUpdateCustomerDTO()
                {
                    Name = "TestName",
                    Surname = "TestSurname",
                    Password = "TestPassword",
                    Email = "test@domain.com"
                };

                var verifyResult = await customerService.CreateNewCustomerAsync(addUpdateCustomerDTO);

                Assert.True(verifyResult.IsSuccess);
                Assert.Equal(System.Net.HttpStatusCode.OK, verifyResult.StatusCode);
                Assert.Equal("test@domain.com", verifyResult.entity!.Email);
            }
        }

        [Fact]
        public async Task CustomerService_SearchAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var unitOfWork = new UnitOfWork(context);

            var customerService = new CustomerService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

            var model = new SieveModel
            {
                Filters = "Name@=Mic"
            };

            var searchResult = await customerService.SearchAsync(model, resultEntity => DomainToDtoMapper.MapGetSingleCustomerDTO(resultEntity));

            Assert.True(searchResult.IsSuccess);
            Assert.Equal(1, searchResult.entityList!.TotalItemCount);
        }
    }
}
