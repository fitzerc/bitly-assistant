using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

namespace BitlyAssistant.DataAccess.Test.Mocks
{
    public class MockConfigurationBuilder
    {
        private readonly string _connectionString;

        public MockConfigurationBuilder(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IConfiguration Build() {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new System.Exception("You need to set your database string!!!");
            }

            var builder = new ConfigurationBuilder();
            var appSettings =
                @"{""ConnectionStrings"" : {
                    ""bitlyPostgres"" : """ + _connectionString + @"""
                }}";

            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

            return builder.Build();
        }
    }
}
