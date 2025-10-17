using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;

namespace ShipParticularsApi.Tests
{
    // NOTE. 스프링에 빈 후처리기 같은 느낌.. 실제 Program 구동시 등록되는 DbContext 관련 의존성을 제거하고, 메모리 디비로 변경
    // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0&pivots=xunit#introduction-to-integration-tests
    // NOTE. WebApplicationFactory를 하나 이상 선언 가능하다는데 .. 스프링부트의 WebMvcTest, SpringBootTest, DataJpaTest가 이런 방식으로 개별 설정한게 아닐까?
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {

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

                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });

                services.AddDbContext<ShipParticularsContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                    options.UseLazyLoadingProxies();
                    options.EnableSensitiveDataLogging();
                });

            });

            builder.UseEnvironment("Development");
        }
    }
}
