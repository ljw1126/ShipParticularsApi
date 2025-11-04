using ShipParticularsApi.Contexts;

namespace ShipParticularsApi.Tests.Tests.Repositories
{
    public interface ITransactionalTest
    {
        ShipParticularsContext Context { get; }
    }
}
