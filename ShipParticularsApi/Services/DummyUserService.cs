namespace ShipParticularsApi.Services
{
    public class DummyUserService : IUserService
    {
        public string GetCurrentUserId()
        {
            return "TEST_USER_01";
        }
    }
}
