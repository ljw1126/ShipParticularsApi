using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using ShipParticularsApi.Contexts;
using Testcontainers.MsSql;

// NOTE. https://www.youtube.com/watch?v=63yG0ZWg8pE
// init과 clean-up을 관리하기 위해 Respawner 라이브러리를 사용
namespace ShipParticularsApi.Tests.Tests
{
    public class WebApplicationFactoryWithTestcontainers
        : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("qwer1234!@#$")
            .WithCleanUp(true)
            .Build();

        private DbConnection _dbConnection = default!;
        private Respawner _respawner = default!;
        private string ConectionString => _dbContainer.GetConnectionString();
        public string ContainerId => $"{_dbContainer.Id}";


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ShipParticularsContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddDbContext<ShipParticularsContext>((container, options) =>
                {
                    options.UseSqlServer(ConectionString);
                    options.UseLazyLoadingProxies();
                    options.EnableSensitiveDataLogging();
                });
            });

            builder.UseEnvironment("Test");
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await MigrateDatabaseAsync();
            await InitializeDbRespawner();
        }

        private async Task MigrateDatabaseAsync()
        {
            // 마이그레이션 실행을 위한 옵션 빌더 생성
            // 마이그레이션 실행은 독립적인 Context 인스턴스로 수행
            await using var context = new ShipParticularsContext(new DbContextOptionsBuilder<ShipParticularsContext>()
                .UseSqlServer(ConectionString)
                .Options);

            // 🌟 Migrate() 호출
            await context.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync().AsTask();
            await base.DisposeAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        private async Task InitializeDbRespawner()
        {
            _dbConnection = new SqlConnection(ConectionString);
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = new[] { "dbo" }
            });
        }
    }
}
