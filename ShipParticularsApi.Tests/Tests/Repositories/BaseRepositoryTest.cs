using Microsoft.EntityFrameworkCore.Storage;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Tests.Tests.Testcontainers;
using Xunit.Abstractions;

namespace ShipParticularsApi.Tests.Tests.Repositories
{
    public abstract class BaseRepositoryTest : ITransactionalTest, IAsyncLifetime
    {
        protected readonly DatabaseFixture _fixture;
        protected ShipParticularsContext _context;
        protected IDbContextTransaction _transaction;
        protected readonly ITestOutputHelper _output;

        protected BaseRepositoryTest(DatabaseFixture fixture, ITestOutputHelper output)
        {
            this._fixture = fixture;
            this._output = output;
        }

        public ShipParticularsContext Context => _context;

        public async Task InitializeAsync()
        {
            _context = _fixture.CreateContext();
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task DisposeAsync()
        {
            _output.WriteLine($"Container Id = {_fixture.ContainerId}");
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }

            await _context.DisposeAsync();
        }
    }
}
