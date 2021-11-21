using BitlyAssistant.Shared.DataAccess;
using BitlyAssistant.Shared.Models;
using Npgsql;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using System.Collections.Generic;

namespace BitlyAssistant.DataAccess.Postgres
{
    public class BitlyRequestPostgresAccess : IBitlyRequestDataAccess
    {
        private readonly BitlyPostgresConnection _dbCon;
        public BitlyRequestPostgresAccess(BitlyPostgresConnection dbCon)
        {
            _dbCon = dbCon;
        }

        public BitlyRequestModel ReadBitlyRequest(int reqId)
        {
            return ReadRequestAsync(reqId).Result;
        }

        private async Task<BitlyRequestModel> ReadRequestAsync(int reqId)
        {
            var param = new Dictionary<NpgsqlTypes.NpgsqlDbType, object>();
            param.Add(NpgsqlTypes.NpgsqlDbType.Integer, reqId);

            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_read_request", param);

            BitlyRequestModel res = null;
            while(dataReader.Read())
            {
                if (res == null && dataReader.FieldCount == 2)
                {
                    res = new BitlyRequestModel()
                    {
                        request_id = dataReader.GetFieldValue<int>(0),
                        request_json = dataReader.GetFieldValue<string>(1)
                    };
                }
            }

            _dbCon.Done(dataReader);

            return res;
        }

        public int WriteBitlyRequest(string requestString)
        {
            return WriteRequestAsync(requestString).Result;
        }

        private async Task<int> WriteRequestAsync(string reqString)
        {
            var param = new Dictionary<NpgsqlTypes.NpgsqlDbType, object>();
            param.Add(NpgsqlTypes.NpgsqlDbType.Text, reqString);

            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_write_request", param);

            int res = -1;
            while(dataReader.Read())
            {
                if (res == -1 && dataReader.FieldCount == 1)
                {
                    res = dataReader.GetFieldValue<int>(0);
                }
            }

            _dbCon.Done(dataReader);

            return res;
        }
    }
}
