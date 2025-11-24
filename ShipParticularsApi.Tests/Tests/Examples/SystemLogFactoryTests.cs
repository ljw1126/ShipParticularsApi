using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class SystemLogFactoryTests
    {
        [Fact]
        public void NullTest()
        {
            string actual = GetAllMessages(null);

            actual.IsNullOrEmpty();
        }

        [Fact]
        public void OverLengthMessageTest()
        {
            var msg1500 = CreateLongMessage(1500);
            var ex = new Exception(msg1500);

            var actual = GetAllMessages(ex);

            actual.Length.Equals(1000);
            actual.Equals(CreateLongMessage(1000));
        }

        [Fact]
        public void InnerMessageTest()
        {
            var outerMsg950 = CreateLongMessage(950);
            var innerMsg100 = CreateLongMessage(100);

            var innerEx = new Exception(innerMsg100);
            var outerEx = new Exception(outerMsg950, innerEx);

            var actual = GetAllMessages(outerEx);

            actual.Length.Equals(1000);
            actual.StartsWith(outerMsg950).Equals(true);

            const string separator = "\r\nInnerException: ";
            actual.Contains(separator).Equals(true);
        }

        private string GetAllMessages(Exception ex, string separator = "\r\nInnerException: ")
        {
            var m_Message = new StringBuilder();

            if (ex != null)
            {
                if (ex.Message.Length >= 1000)
                    m_Message.Append(ex.Message.Substring(0, 1000));
                else
                    m_Message.Append(ex.Message);
            }

            if (ex?.InnerException != null)
            {
                m_Message.Append(separator);

                if (ex.InnerException.Message.Length >= 1000)
                    m_Message.Append(ex.InnerException.Message.Substring(0, 1000));
                else
                    m_Message.Append(ex.InnerException.Message);
            }

            if (m_Message.Length >= 1000)
            {
                m_Message.Remove(1000, m_Message.Length - 1000);
                return m_Message.ToString();
            }
            else
            {
                return m_Message.ToString();
            }
        }

        private string CreateLongMessage(int length) => new string('X', length);
    }
}
