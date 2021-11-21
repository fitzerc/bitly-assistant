using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.Shared.DataAccess;
using BitlyAssistant.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitlyAssistant.DataAccess.Postgres
{
    public class ShortLinkPostgresAccess : IShortLinkDataAccess
    {
        private readonly BitlyPostgresConnection _dbCon;

        public ShortLinkPostgresAccess(BitlyPostgresConnection dbCon)
        {
            _dbCon = dbCon;
        }

        public List<ShortLinkModel> ReadAllShortLinks()
        {
            return ReadAllShortLinksAsync().Result;
        }

        private async Task<List<ShortLinkModel>> ReadAllShortLinksAsync()
        {
            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_read_all_short_links");

            List<ShortLinkModel> res = new List<ShortLinkModel>();

            if (dataReader.FieldCount != 7)
            {
                return res;
            }

            while(dataReader.Read())
            {
                res.Add(new ShortLinkModel()
                {
                    short_link_id = dataReader.GetFieldValue<int>(0),
                    short_link = dataReader.GetFieldValue<string>(1),
                    long_link = dataReader.GetFieldValue<string>(2),
                    description = dataReader.GetFieldValue<string>(3),
                    request_id = dataReader.GetFieldValue<int>(4),
                    response_id = dataReader.GetFieldValue<int>(5),
                    date_added = dataReader.GetFieldValue<DateTime>(6)
                });
            }

            _dbCon.Done(dataReader);

            return res;
        }

        public ShortLinkModel ReadShortLink(int linkId)
        {
            return ReadShortLinkAsync(linkId).Result;
        }

        private async Task<ShortLinkModel> ReadShortLinkAsync(int linkId)
        {
            var param = new List<(NpgsqlTypes.NpgsqlDbType, object)>() { (NpgsqlTypes.NpgsqlDbType.Integer, linkId) };

            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_read_short_link", param);

            ShortLinkModel res = null;
            while(dataReader.Read())
            {
                //short_link_id, short_link, long_link, description, request_id, response_id, date_added
                if (res == null && dataReader.FieldCount == 7)
                {
                    res = new ShortLinkModel()
                    {
                        short_link_id = dataReader.GetFieldValue<int>(0),
                        short_link = dataReader.GetFieldValue<string>(1),
                        long_link = dataReader.GetFieldValue<string>(2),
                        description = dataReader.GetFieldValue<string>(3),
                        request_id = dataReader.GetFieldValue<int>(4),
                        response_id = dataReader.GetFieldValue<int>(5),
                        date_added = dataReader.GetFieldValue<DateTime>(6)
                    };
                }
            }

            _dbCon.Done(dataReader);

            return res;
        }

        public int WriteShortLink(ShortLinkModel link)
        {
            return WriteShortLinkAsync(link).Result;
        }

        public async Task<int> WriteShortLinkAsync(ShortLinkModel link)
        {
            var param = new List<(NpgsqlTypes.NpgsqlDbType, object)>();
            param.Add((NpgsqlTypes.NpgsqlDbType.Text, link.short_link));
            param.Add((NpgsqlTypes.NpgsqlDbType.Text, link.long_link));
            param.Add((NpgsqlTypes.NpgsqlDbType.Text, link.description));
            param.Add((NpgsqlTypes.NpgsqlDbType.Integer, link.request_id));
            param.Add((NpgsqlTypes.NpgsqlDbType.Integer, link.response_id));

            var dataReader = await _dbCon.CallFunctionGetDataReaderAsync("func_write_short_link", param);

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
    }
}
