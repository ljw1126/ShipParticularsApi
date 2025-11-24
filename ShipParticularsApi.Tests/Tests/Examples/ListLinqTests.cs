using FluentAssertions;

namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class ListLinqTests
    {
        [Fact(DisplayName = "TIME_STAMP 중복 제외")]
        public void RemoveDuplicateDataTest()
        {
            // Arrange
            // (ID와 TIME_STAMP는 유일해야 합니다. ID는 비교하지 않습니다.)
            var dataWithDuplicates = new List<Dictionary<string, dynamic>>
            {
                new Dictionary<string, dynamic> { { "ID", 1 }, { "TIME_STAMP", "2025-11-18T10:00:00" }, { "Value", "A" } },
                new Dictionary<string, dynamic> { { "ID", 2 }, { "TIME_STAMP", "2025-11-18T10:10:00" }, { "Value", "B" } },
                new Dictionary<string, dynamic> { { "ID", 3 }, { "TIME_STAMP", "2025-11-18T10:00:00" }, { "Value", "C" } }, // 10:00 중복
                new Dictionary<string, dynamic> { { "ID", 4 }, { "TIME_STAMP", "2025-11-18T10:20:00" }, { "Value", "D" } },
                new Dictionary<string, dynamic> { { "ID", 5 }, { "TIME_STAMP", "2025-11-18T10:10:00" }, { "Value", "E" } }  // 10:10 중복
            };

            // 기대되는 결과: 첫 번째 중복된 요소만 유지되어야 합니다 (ID 1, 2, 4)
            var expectedTimeStamps = new[]
            {
                "2025-11-18T10:00:00", // ID 1 (첫 번째 등장)
                "2025-11-18T10:10:00", // ID 2 (첫 번째 등장)
                "2025-11-18T10:20:00"
            };

            // Act
            var resultList = RemoveDuplicateDataByUsingLinq(dataWithDuplicates);

            // Assert
            // 1. 결과 리스트의 크기가 예상대로 3인지 확인
            resultList.Should().HaveCount(3);

            // 2. 결과 리스트에 중복이 제거된 TIME_STAMP 값만 포함되어 있는지 확인
            resultList.Select(d => d["TIME_STAMP"].ToString())
                .Should().BeEquivalentTo(expectedTimeStamps);

            // 3. 그룹화 시 첫 번째 요소(ID)가 제대로 선택되었는지 확인
            resultList.Select(d => d["ID"])
                .Should().BeEquivalentTo(new[] { 1, 2, 4 });
        }

        // NOTE. TIME_STAMP 기준 정렬되어 있지 않으면 올바르게 동작하지 않음
        private List<Dictionary<string, dynamic>> RemoveDuplicateData(List<Dictionary<string, dynamic>> dataList)
        {
            List<Dictionary<string, dynamic>> resultList = new List<Dictionary<string, dynamic>>();
            foreach (var item in dataList)
            {
                if (resultList.Any() == false)
                {
                    resultList.Add(item);
                    continue;
                }

                string time1 = (string)resultList.Last()["TIME_STAMP"];
                string time2 = (string)item["TIME_STAMP"];

                if (time1 != time2)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }

        // NOTE. TIME_STAMP 기준 정렬 후 중복 제거
        private List<Dictionary<string, dynamic>> RemoveDuplicateDataOrderByTimeStamp(List<Dictionary<string, dynamic>> dataList)
        {
            List<Dictionary<string, dynamic>> resultList = new List<Dictionary<string, dynamic>>();
            foreach (var item in dataList.OrderBy(d => d["TIME_STAMP"]).ToList())
            {
                if (resultList.Any() == false)
                {
                    resultList.Add(item);
                    continue;
                }

                string time1 = (string)resultList.Last()["TIME_STAMP"];
                string time2 = (string)item["TIME_STAMP"];

                if (time1 != time2)
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }

        private List<Dictionary<string, dynamic>> RemoveDuplicateDataByUsingLinq(List<Dictionary<string, dynamic>> dataList)
        {
            return dataList
                .GroupBy(d => d["TIME_STAMP"])
                .Select(g => g.First())
                .ToList();
        }
    }
}
