using Galaxy.Libra.DapperExtensions.DBBuilder.MySQL;
using Galaxy.Libra.DapperExtensions.Sql;
using Galaxy.Libra.DapperExtensions.Test.Entity;
using MySql.Data.MySqlClient;
using Xunit;

namespace Galaxy.Libra.DapperExtensions.Test
{
    public class DBBuilderTest
    {
        private string conStr = $"server=192.168.65.228;port=3307;user=root;password=123456;SslMode=None;";
        private string conStr2 = $"server=192.168.65.228;port=3307;database=Test888;user=root;password=123456;SslMode=None;";

        [Fact]
        public void Test()
        {
            Galaxy.Libra.DapperExtensions.DapperExtensions.SqlDialect = new MySqlDialect();
            MySqlConnection conn = new MySqlConnection(conStr);
            MySQLDBBuilder mySQLDBBuilder = new MySQLDBBuilder(conn);
            mySQLDBBuilder.CreateDataBase("Test888");

            MySqlConnection conn2 = new MySqlConnection(conStr2);
            MySQLTableBuilder mySQLTableBuilder = new MySQLTableBuilder(conn2);
            mySQLTableBuilder.CreateTable(typeof(TestEntity), new TestEntityMapper());
        }
    }
}
