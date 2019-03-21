using Galaxy.Libra.DapperExtensions.EntityRepository;
using Galaxy.Libra.DapperExtensions.Sql;
using Galaxy.Libra.DapperExtensions.Test.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Galaxy.Libra.DapperExtensions.Test
{
    public class UserRepositoryTest
    {
        [Fact]
        public void Test()
        {
            TestEntityRepository testEntityRepository = new TestEntityRepository();

            TestEntity t = new TestEntity
            {
                Name = "test",
                TestInt = 1
            };

            testEntityRepository.Add(t);
            TestEntity t2 = testEntityRepository.GetList(u => u.Name == "test")?.FirstOrDefault();
            testEntityRepository.Delete(t3 => t3.Id == t2.Id);
            testEntityRepository.Delete(t3 => t3.Name == t2.Name);
        }
    }

    public class TestEntityRepository : BaseEntityRepository<TestEntity>
    {
        private string conStr = $"server=192.168.65.228;port=3307;database=Test888;user=root;password=123456;SslMode=None;";

        public TestEntityRepository()
        {
            Galaxy.Libra.DapperExtensions.DapperExtensions.SqlDialect = new MySqlDialect();
            curDbConnection = new MySqlConnection(conStr);
        }
    }
}
