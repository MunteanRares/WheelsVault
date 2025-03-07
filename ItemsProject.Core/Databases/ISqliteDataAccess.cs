﻿
namespace ItemsProject.Core.Databases
{
    public interface ISqliteDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName);
        Task SaveData<T>(string sqlStatement, T parameters, string connectionStringName);
    }
}