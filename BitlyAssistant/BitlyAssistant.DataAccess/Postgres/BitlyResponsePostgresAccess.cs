using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.Shared.DataAccess;
using BitlyAssistant.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitlyAssistant.DataAccess.Postgres
{
    public class BitlyResponsePostgresAccess : IBitlyResponseDataAccess
    {
        private readonly BitlyPostgresConnection _dbCon;

        public BitlyResponsePostgresAccess(BitlyPostgresConnection dbCon)
        {
            _dbCon = dbCon;
        }

        public int WriteBitlyResponse(string responseString)
        {
            return WriteBitlyResponseAsync(responseString).Result;
        }

        private async Task<int> WriteBitlyResponseAsync(string responseString)
        {
            var param = new Dictionary<NpgsqlTypes.NpgsqlDbType, object>();
            param.Add(NpgsqlTypes.NpgsqlDbType.Text, responseString);

            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_write_response", param);

            var res = -1;
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

        public BitlyResponseModel ReadBitlyResponse(int respId)
        {
            return ReadBitlyResponseAsync(respId).Result;
        }

        private async Task<BitlyResponseModel> ReadBitlyResponseAsync(int respId)
        {
            var param = new Dictionary<NpgsqlTypes.NpgsqlDbType, object>();
            param.Add(NpgsqlTypes.NpgsqlDbType.Integer, respId);

            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_read_response", param);

            BitlyResponseModel res = null;
            while(dataReader.Read())
            {
                if (res == null && dataReader.FieldCount == 2)
                {
                    res = new BitlyResponseModel()
                    {
                        response_id = dataReader.GetFieldValue<int>(0),
                        response_json = dataReader.GetFieldValue<string>(1)
                    };
                }
            }

            _dbCon.Done(dataReader);

            return res;
        }


    }
}
