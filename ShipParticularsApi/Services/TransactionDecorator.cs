using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Exceptions;
using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Services.Dtos.Results;

namespace ShipParticularsApi.Services
{
    public class TransactionDecorator(ShipParticularsContext dbContext, IShipParticularsService target)
        : IShipParticularsService
    {
        public async Task Create(ShipParticularsParam param)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await target.Create(param);

                await dbContext.SaveChangesAsync();
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

        public async Task<bool> Upsert(ShipParticularsParam param)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                bool isNewResource = await target.Upsert(param);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return isNewResource;
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
            return target.GetShipParticulars(shipKey);
        }

    }
}
