using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Services.Dtos.Results;

namespace ShipParticularsApi.Services
{
    public interface IShipParticularsService
    {
        Task Process(ShipParticularsParam param);
        Task<ShipParticularsResult> GetShipParticulars(string shipKey);
    }
}
