using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Databases;

namespace ItemsProject.Core.Data
{
    public class SqliteDatabaseInitializer
    {
        public static void SetDefaultFolder(ISqliteDataAccess db, string connectionStringName)
        {
            string sqlStatement = "insert into Folders (name, IsDefault) " +
                                   "select 'All Cars', 1 " +
                                   "where not exists (select 1 from Folders where isDefault = 1);";
            db.SaveData(sqlStatement, new { }, connectionStringName);

            sqlStatement = """
                    INSERT INTO AppSettings (isDbPopulated)
                    SELECT 0
                    WHERE NOT EXISTS (SELECT 1 FROM AppSettings);
                """;

            db.SaveData(sqlStatement, new { }, connectionStringName);
        }
    }
}
