using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Services
{
    public interface IShipServiceRepository
    {
        Task<ShipService> GetByShipKeyAndServiceNameAsync(string shipKey, ServiceNameTypes satAis);
    }
}
