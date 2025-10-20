using ShipParticularsApi.Contexts;
using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Services.Dtos.Results;

namespace ShipParticularsApi.Services
{
    public class TransactionDecorator : IShipParticularsService
    {
        private readonly ShipParticularsContext _dbContext;
        private readonly IShipParticularsService _target;

        public TransactionDecorator(ShipParticularsContext dbContext, IShipParticularsService target)
        {
            _dbContext = dbContext;
            _target = target;
        }

        public async Task Process(ShipParticularsParam param)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _target.Process(param);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<ShipParticularsResult> GetShipParticulars(string shipKey)
        {
            return _target.GetShipParticulars(shipKey);
        }
    }
}
