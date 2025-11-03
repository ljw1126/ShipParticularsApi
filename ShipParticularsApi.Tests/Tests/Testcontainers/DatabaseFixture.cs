using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using Testcontainers.MsSql;

namespace ShipParticularsApi.Tests.Tests.Testcontainers
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("qwer1234!@#$")
            .WithCleanUp(true)
            .Build();

        public string ConectionString => _dbContainer.GetConnectionString();
        public string ContainerId => $"{_dbContainer.Id}";

        private DbContextOptions<ShipParticularsContext> _options;
        public ShipParticularsContext CreateContext() => new(_options);

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            _options = new DbContextOptionsBuilder<ShipParticularsContext>()
                           .UseSqlServer(_dbContainer.GetConnectionString())
                           .UseLazyLoadingProxies()
                           .Options;

            var context = new ShipParticularsContext(_options);
            context.Database.Migrate();
        }

        public Task DisposeAsync() => _dbContainer.DisposeAsync().AsTask();
    }
}
