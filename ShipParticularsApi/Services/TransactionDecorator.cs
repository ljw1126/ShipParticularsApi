using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Exceptions;
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

        public async Task Create(ShipParticularsParam param)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _target.Create(param);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();

                throw new DatabaseConstraintException("데이터베이스 처리 중 예기치 않은 오류가 발생했습니다. 관리자에게 문의해주세요.", e);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task Upsert(ShipParticularsParam param)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _target.Upsert(param);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();

                throw new DatabaseConstraintException("데이터베이스 처리 중 예기치 않은 오류가 발생했습니다. 관리자에게 문의해주세요.", e);
            }
            catch (Exception)
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
