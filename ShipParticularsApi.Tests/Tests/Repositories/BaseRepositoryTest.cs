using ShipParticularsApi.Contexts;
using ShipParticularsApi.Tests.Tests.Testcontainers;
using Xunit.Abstractions;

namespace ShipParticularsApi.Tests.Tests.Repositories
{
    // NOTE. IAsyncLifetime은 클래스 단위로 초기화/종료를 선언하다보니 동일한 DbContext 사용하게 됨 => 테스트 메서드별 개별 트랜잭션 관리 위해 사용하지 않음
    public abstract class BaseRepositoryTest : ITransactionalTest
    {
        protected readonly DatabaseFixture _fixture;
        protected readonly ITestOutputHelper _output;

        protected BaseRepositoryTest(DatabaseFixture fixture, ITestOutputHelper output)
        {
            this._fixture = fixture;
            this._output = output;
        }

        public ShipParticularsContext Context => this._fixture.CreateContext();
    }
}
