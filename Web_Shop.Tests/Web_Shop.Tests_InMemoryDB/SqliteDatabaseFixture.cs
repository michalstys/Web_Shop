using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWSI_Shop.Persistence.MySQL.Context;
using WWSI_Shop.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop.Tests_InMemoryDB
{
    public class SqliteDatabaseFixture : IDisposable
    {
        private readonly SqliteConnection connection;
        public SqliteDatabaseFixture()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
        }
        public void Dispose() => this.connection.Dispose();
        public WwsishopContext CreateContext()
        {
            var context = new WwsishopContext(new DbContextOptionsBuilder<WwsishopContext>()
                .UseSqlite(this.connection)
                .Options);


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

            return context;
        }
    }
}
