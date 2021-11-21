using BitlyAssistant.DataAccess.Postgres;
using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.DataAccess.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitlyAssistant.DataAccess.Test.Postgres
{
    [TestClass]
    public class BitlyResponsePostgresAccessTests
    {
        [TestMethod]
        public void ReadWriteBitlyResponseTest()
        {
            var mockConfigBuilder = new MockConfigurationBuilder(Constants.DB_CONNECTION_STRING);

            var dbConWrapper = new BitlyPostgresConnection(mockConfigBuilder.Build());

            var dataAccess = new BitlyResponsePostgresAccess(dbConWrapper);
            var resp = "{\"test\": \"testresponse\"}";

            var respId = dataAccess.WriteBitlyResponse(resp);

            var resp2 = dataAccess.ReadBitlyResponse(respId).response_json;

            Assert.AreEqual(resp, resp2);
            //Add delete logic here at some point
        }
    }
}
