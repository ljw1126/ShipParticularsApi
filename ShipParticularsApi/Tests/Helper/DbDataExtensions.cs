using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Helper
{
    public static class DbDataExtensions
    {
        // NOTE. MigrateAsync 실행시 SQLite랑 MSSQL Syntax가 틀려서 오류 발생
        // NOTE. 통합 테스트에서 사용시 생성자에 async 키워드를 붙이지 못해 메서드마다 호출해야 하므로 중복 발생 (await _factory.Services.InitializeDatabaseAsync();)
        public static async Task InitializeDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShipParticularsContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        public static async Task SeedDataAsync(this IServiceProvider services, ShipInfo initData)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShipParticularsContext>();

            context.ShipInfos.Add(initData);
            await context.SaveChangesAsync();
        }
    }
}
