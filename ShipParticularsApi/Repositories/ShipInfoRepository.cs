using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Services;

namespace ShipParticularsApi.Repositories
{
    public class ShipInfoRepository(ShipParticularsContext context) : IShipInfoRepository
    {
        public Task<ShipInfo> GetByShipKeyAsync(string shipKey)
        {
            throw new NotImplementedException();
        }

        public Task<ShipInfo> UpsertAsync(ShipInfo shipInfo)
        {
            throw new NotImplementedException();
        }
    }
}
