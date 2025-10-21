using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Repositories
{
    public class ShipInfoRepository(ShipParticularsContext context) : IShipInfoRepository
    {
        public async Task<ShipInfo?> GetByShipKeyAsync(string shipKey)
        {
            return await context.ShipInfos
                 .Include(s => s.ShipServices)
                 .Include(s => s.ShipSatellite)
                 .Include(s => s.SkTelinkCompanyShip)
                 .Include(s => s.ReplaceShipName)
                 .Include(s => s.ShipModelTest)
                 .AsSplitQuery()
                 .SingleOrDefaultAsync(s => s.ShipKey == shipKey && s.IsService == true);
        }

        public Task UpsertAsync(ShipInfo shipInfo)
        {
            if (shipInfo.Id == 0)
            {
                context.ShipInfos.Add(shipInfo);
            }
            else
            {
                context.ShipInfos.Update(shipInfo);
            }

            return Task.CompletedTask;
        }

        public async Task<ShipInfo?> GetReadOnlyByShipKeyAsync(string shipKey)
        {
            return await context.ShipInfos
                 .AsNoTracking()
                 .Include(s => s.ShipServices)
                 .Include(s => s.ShipSatellite)
                 .Include(s => s.SkTelinkCompanyShip)
                 .Include(s => s.ReplaceShipName)
                 .Include(s => s.ShipModelTest)
                 .AsSplitQuery()
                 .SingleOrDefaultAsync(s => s.ShipKey == shipKey && s.IsService == true);
        }

    }
}
