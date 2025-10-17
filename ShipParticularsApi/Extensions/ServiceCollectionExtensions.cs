using ShipParticularsApi.Contexts;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Services;

namespace ShipParticularsApi.Extensions
{
    // NOTE. Naming Convention 존재, *Extension으로 클래스명 지으면 Program.cs에서 호출 x
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IShipInfoRepository, ShipInfoRepository>();
            services.AddScoped<ShipParticularsService>();
            services.AddScoped<IShipParticularsService>(
                s => new TransactionDecorator(
                    s.GetRequiredService<ShipParticularsContext>(),
                    s.GetRequiredService<ShipParticularsService>()
                ));
            return services;
        }
    }
}
