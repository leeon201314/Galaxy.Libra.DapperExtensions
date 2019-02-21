using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Sql;
using Galaxy.Libra.DapperExtensions.Test.Entity;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Galaxy.Libra.DapperExtensions.Test
{
    public class SqlGeneratorUnitTest
    {
        [Fact]
        public void Test()
        {
            IDapperExtensionsConfiguration conf = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect());
            SqlGeneratorImpl sqlGeneratorImpl = new SqlGeneratorImpl(conf);

            IFieldPredicate nameFieldPredicate = Predicates.Field<User>(p => p.Name, Operator.Like, "不知道%");
            List<ISort> sortList = new List<ISort>();
            sortList.Add(new Sort() { PropertyName = "Name", Ascending = true });
            string selectPagedSql = sqlGeneratorImpl.SelectPaged(new UserMapper(), nameFieldPredicate, sortList, 20, 2, new Dictionary<string, object>());
            string res = "SELECT `User`.`Id`, `User`.`Name`, `User`.`Psw`, `User`.`RoleId` FROM `User` WHERE (`User`.`Name` LIKE @Name_0) ORDER BY `User`.`Name` ASC LIMIT @firstResult, @maxResults";
            Assert.Equal(selectPagedSql, res);
        }
    }
}
