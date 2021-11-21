using BitlyAssistant.DataAccess.Postgres;
using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.DataAccess.Test.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace BitlyAssistant.DataAccess.Test.Postgres
{
    [TestClass]
    public class BitlyRequestPostgresAccessTests
    {
        [TestMethod]
        public void ReadWriteBitlyRequestTest()
        {
            var mockConfigBuilder = new MockConfigurationBuilder(Constants.DB_CONNECTION_STRING);
            var dbConWrapper = new BitlyPostgresConnection(mockConfigBuilder.Build());

            var dataAccess = new BitlyRequestPostgresAccess(dbConWrapper);
            var req = "{\"test\": \"testvalues\"}";

            var reqId = dataAccess.WriteBitlyRequest(req);

            var req2 = dataAccess.ReadBitlyRequest(reqId).request_json;

            Assert.AreEqual(req, req2);
            //Add delete logic here at some point
        }
    }
}
