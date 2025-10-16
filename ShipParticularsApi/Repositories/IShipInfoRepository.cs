using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Repositories
{
    public interface IShipInfoRepository
    {
        Task<ShipInfo?> GetByShipKeyAsync(string shipKey);
        Task UpsertAsync(ShipInfo shipInfo);
    }
}
