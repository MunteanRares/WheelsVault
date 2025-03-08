using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace ItemsProject.Core.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoreProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;
            if (isStoreProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(sqlStatement, parameters, commandType: commandType);
                return rows.ToList();
            }
        }

        public async Task SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoreProcedurue = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;
            if (isStoreProcedurue == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
