using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Services;

namespace ShipParticularsApi.Repositories
{
    public class ShipServiceRepository(ShipParticularsContext context) : IShipServiceRepository
    {
        public Task<ShipService> GetByShipKeyAndServiceNameAsync(string shipKey, ServiceNameTypes satAis)
        {
            throw new NotImplementedException();
        }
    }
}
