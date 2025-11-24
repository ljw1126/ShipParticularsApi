namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class OutKeywordTests
    {
        [Fact]
        public void CalculateAndReport_ShouldAssignValueToOutParameters()
        {
            // Arrange
            int inputA = 5;
            int inputB = 8;

            // 💡 1. out 변수는 초기화 없이 선언만 합니다.
            //    (C# 컴파일러가 허용하며, out의 핵심 특징입니다.)
            int actualSum;
            string actualStatusCode;

            // Act
            // 💡 2. 메서드 호출 시 out 키워드를 사용해 변수를 전달합니다.
            CalculateAndReport(inputA, inputB, out actualSum, out actualStatusCode);

            // Assert
            // 💡 3. 호출 후, out 변수에 값이 올바르게 할당되었는지 확인합니다.

            // Sum 검증 (5 + 8 = 13)
            Assert.Equal(13, actualSum);

            // StatusCode 검증 (13 > 10 이므로 SUCCESS_HIGH)
            Assert.Equal("SUCCESS_HIGH", actualStatusCode);
        }

        [Fact]
        public void CalculateAndReport_WithDifferentInput_ShouldReturnCorrectValues()
        {
            // Arrange
            int inputA = 2;
            int inputB = 3;

            // out 변수 선언
            int actualSum;
            string actualStatusCode;

            // Act
            CalculateAndReport(inputA, inputB, out actualSum, out actualStatusCode);

            // Assert
            // Sum 검증 (2 + 3 = 5)
            Assert.Equal(5, actualSum);

            // StatusCode 검증 (5 <= 10 이므로 SUCCESS_LOW)
            Assert.Equal("SUCCESS_LOW", actualStatusCode);
        }

        // out 키워드 사용: sum과 statusCode에 반드시 값을 할당해야 함
        public void CalculateAndReport(int a, int b, out int sum, out string statusCode)
        {
            sum = a + b;

            if (sum > 10)
            {
                statusCode = "SUCCESS_HIGH";
            }
            else
            {
                statusCode = "SUCCESS_LOW";
            }
        }

        public void NoInitOutParameter(int a, int b, out int sum, out string statusCode)
        {
            sum = -1; // 초기화 하지 않을 경우 컴파일 에러가 발생
            statusCode = "SUCCESS";
        }
    }
}
