using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Services.Dtos.Results;

namespace ShipParticularsApi.Services
{
    public interface IShipParticularsService
    {
        Task Create(ShipParticularsParam param);
        Task<bool> Upsert(ShipParticularsParam param);
        Task<ShipParticularsResult> GetShipParticulars(string shipKey);
    }
}
