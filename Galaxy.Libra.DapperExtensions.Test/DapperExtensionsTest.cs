using Galaxy.Libra.DapperExtensions.Sql;
using Galaxy.Libra.DapperExtensions.Test.Entity;
using MySql.Data.MySqlClient;
using System.Data;
using Xunit;

namespace Galaxy.Libra.DapperExtensions.Test
{
    public class DapperExtensionsTest
    {
        private string conStr = $"server=192.168.65.228;port=3307;database=Test123;user=root;password=123456;SslMode=None;";

        [Fact]
        public void TestGet()
        {
            Galaxy.Libra.DapperExtensions.DapperExtensions.SqlDialect = new MySqlDialect();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                conn.Delete<User>(u => u.Id == 1);
                User user = new User { Id = 1, Name = "123", Psw = "123", RoleId = "123" };
                conn.Insert<User>(user);
                var res = conn.GetList<User>(u => u.Name == "123");
                conn.Delete<User>(user);
                conn.Close();

                Assert.NotNull(res);
            }
        }
    }
}
