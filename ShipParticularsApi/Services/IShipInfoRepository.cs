using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Services
{
    public interface IShipInfoRepository
    {
        Task<ShipInfo> GetByShipKeyAsync(string shipKey);
        Task<ShipInfo> UpsertAsync(ShipInfo shipInfo);
    }
}
