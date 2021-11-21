using BitlyAssistant.DataAccess.Postgres;
using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.DataAccess.Test.Mocks;
using BitlyAssistant.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitlyAssistant.DataAccess.Test.Postgres
{
    [TestClass]
    public class ShortLinkPostgresAccessTests
    {
        [TestMethod]
        public void ReadWriteShortLinkTest()
        {
            var mockConfigBuilder = new MockConfigurationBuilder(Constants.DB_CONNECTION_STRING);
            var dbConWrapper = new BitlyPostgresConnection(mockConfigBuilder.Build());
            var dataAccess = new ShortLinkPostgresAccess(dbConWrapper);

            var desc = "some description";
            var lng_link = "veryshort.short.com";
            var shrt_link = "short";
            var req_id = 1;
            var resp_id = 1;


            var resp = new ShortLinkModel()
            {
                description = desc,
                long_link = lng_link,
                short_link = shrt_link,
                request_id = req_id,
                response_id = resp_id
            };

            var respId = dataAccess.WriteShortLink(resp);
            /*
            Assert.AreEqual(4, respId);
            */

            var resp2 = dataAccess.ReadShortLink(respId);
            Assert.AreEqual(resp.short_link, resp2.short_link);
        }

        [TestMethod]
        public void ReadAllShortLinksTest()
        {
            var mockConfigBuilder = new MockConfigurationBuilder(Constants.DB_CONNECTION_STRING);
            var dbConWrapper = new BitlyPostgresConnection(mockConfigBuilder.Build());
            var dataAccess = new ShortLinkPostgresAccess(dbConWrapper);

            var resp = dataAccess.ReadAllShortLinks();

            Assert.IsTrue(resp.Count > 0);
        }
    }
}
