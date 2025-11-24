namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class RefKeywordTests
    {

        [Fact]
        public void AccTest()
        {
            int a = 9;
            AddOne(ref a);

            Assert.Equal(10, a);

            AddOne(ref a);
            Assert.Equal(11, a);
        }

        private void AddOne(ref int number)
        {
            number += 1;
        }

        [Fact]
        public void RepeatAddTest()
        {
            int a = 0;
            for (int i = 1; i <= 10; i++)
            {
                AddOne(ref a);
            }

            Assert.Equal(10, a);
        }
    }
}
