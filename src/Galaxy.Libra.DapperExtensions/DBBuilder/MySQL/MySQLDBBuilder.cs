using Dapper;
using System.Data;

namespace Galaxy.Libra.DapperExtensions.DBBuilder.MySQL
{
    public class MySQLDBBuilder : IDataBaseBuilder
    {
        protected IDbConnection curDbConnection = null;

        public MySQLDBBuilder(IDbConnection dbConnection)
        {
            curDbConnection = dbConnection;
        }

        public void CreateDataBase(string dbName)
        {
            if (curDbConnection != null)
            {
                using (curDbConnection)
                {
                    if (curDbConnection.State != ConnectionState.Open)
                        curDbConnection.Open();

                    curDbConnection.Execute($"Create Database If Not Exists {dbName} Character Set utf8mb4;");

                    curDbConnection.Close();
                }
            }
        }
    }
}
