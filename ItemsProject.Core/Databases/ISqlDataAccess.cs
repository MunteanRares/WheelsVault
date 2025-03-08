


namespace ItemsProject.Core.Databases
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoreProcedure = false);
        Task SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoreProcedurue = false);
    }
}