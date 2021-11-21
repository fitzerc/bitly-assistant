using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitlyAssistant.DataAccess.Postgres.PostgresWrapper
{
    public class BitlyPostgresConnection
    {
        private const string CON_STRING_NAME = "bitlyPostgres";
        private readonly IConfiguration _config;
        private NpgsqlTransaction _currentTran;
        private NpgsqlCommand _currentCmd;
        private NpgsqlDataReader _currentReader;

        public NpgsqlConnection Connection { get; }

        public BitlyPostgresConnection(IConfiguration configuration)
        {
            _config = configuration;
            Connection = new NpgsqlConnection(_config.GetConnectionString(CON_STRING_NAME));
        }

        public async Task<NpgsqlDataReader> CallFunctionGetDataReaderAsync(string funcName, Dictionary<NpgsqlDbType, object> parameters)
        {
            Connection.Open();
            BeginTransaction();
            var cmd = GetFunctionCallCommand(funcName, parameters);

            return await ExecuteCommandAsync(cmd);
        }

        public async Task<NpgsqlDataReader> CallFunctionGetDataReaderAsync(string funcName, List<(NpgsqlDbType, object)> parameters)
        {
            Connection.Open();
            BeginTransaction();
            var cmd = GetFunctionCallCommand(funcName, parameters);

            return await ExecuteCommandAsync(cmd);
        }

        public async Task<NpgsqlDataReader> CallFunctionGetDataReaderAsync(string funcName)
        {
            Connection.Open();
            BeginTransaction();
            var cmd = GetFunctionCallCommand(funcName);

            return await ExecuteCommandAsync(cmd);
        }

        public NpgsqlTransaction BeginTransaction()
        {
            return _currentTran = Connection.BeginTransaction();
        }

        public NpgsqlCommand GetFunctionCallCommand(string funcName)
        {
            return new NpgsqlCommand(funcName, Connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
        }

        public NpgsqlCommand GetFunctionCallCommand(string funcName, Dictionary<NpgsqlTypes.NpgsqlDbType, object> parameters)
        {
            var cmd = new NpgsqlCommand(funcName, Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            foreach(var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            return _currentCmd = cmd;
        }

        public NpgsqlCommand GetFunctionCallCommand(string funcName, List<(NpgsqlTypes.NpgsqlDbType, object)> parameters)
        {
            var cmd = new NpgsqlCommand(funcName, Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            foreach(var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Item1, param.Item2);
            }

            return _currentCmd = cmd;
        }

        public NpgsqlDataReader ExecuteCommand(NpgsqlCommand cmd)
        {
            return _currentReader = ExecuteCommandAsync(cmd).Result;
        }

        private async Task<NpgsqlDataReader> ExecuteCommandAsync(NpgsqlCommand cmd)
        {
            return await cmd.ExecuteReaderAsync();
        }

        public void Open()
        {
            Connection?.Close();
            Connection?.Open();
        }

        public void Close()
        {
            Connection?.Close();
        }

        public void Done(NpgsqlDataReader reader)
        {
            reader.Close();
            _currentTran.Commit();
            Close();
        }
    }
}
