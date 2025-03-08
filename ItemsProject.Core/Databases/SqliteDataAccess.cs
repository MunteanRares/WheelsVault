using System.Data;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ItemsProject.Core.Databases
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        private readonly IConfiguration _config;
        public SqliteDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(sqlStatement, parameters);
                return rows.ToList();
            }
        }

        public async Task SaveData<T>(string sqlStatement, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.ExecuteAsync(sqlStatement, parameters);
            }
        }
    }
}
