using ShipParticularsApi.Services.Dtos;

namespace ShipParticularsApi.Services
{
    public interface IShipParticularsService
    {
        Task Process(ShipParticularsParam param);
    }
}
