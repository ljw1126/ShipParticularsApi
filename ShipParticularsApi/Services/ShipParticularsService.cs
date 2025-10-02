using ShipParticularsApi.Contexts;

namespace ShipParticularsApi.Services
{
    public class ShipParticularsService(ShipParticularsContext dbContext,
        IReplaceShipNameRepository replaceShipNameRepository,
        IShipInfoRepository shipInfoRepository,
        IShipModelTestRepository shipModelTestRepository,
        IShipSatelliteRepository shipSatelliteRepository,
        IShipServiceRepository shipServiceRepository,
        ISkTelinkCompanyShipRepository skTelinkCompanyShipRepository)
    {
        public async Task Process(Object param)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                // 비즈니스 로직 작성

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
