using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using Galaxy.Libra.DapperExtensions.Sql;
using Galaxy.Libra.DapperExtensions.Test.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Galaxy.Libra.DapperExtensions.Test
{
    public class PredicatesUnitTest
    {
        [Fact]
        public void MySQLBuildTest()
        {
            IDapperExtensionsConfiguration conf = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect());
            SqlGeneratorImpl sqlGeneratorImpl = new SqlGeneratorImpl(conf);

            IFieldPredicate nameFieldPredicate = Predicates.Field<User>(p => p.Name, Operator.Like, "²»ÖªµÀ%");
            string namesql = nameFieldPredicate.GetSql(sqlGeneratorImpl, new Dictionary<string, object>());
            Assert.Equal("(`User`.`Name` LIKE @Name_0)", namesql);

            List<string> valueList = new List<string>() { "1", "2", "3" };
            IFieldPredicate nameFieldPredicate2 = Predicates.Field<User>(p => p.Name, Operator.Eq, valueList);
            string namesql2 = nameFieldPredicate2.GetSql(sqlGeneratorImpl, new Dictionary<string, object>());
            Assert.Equal("(`User`.`Name` IN (@Name_0, @Name_1, @Name_2))", namesql2);

            IBetweenPredicate idFieldPredicate = Predicates.Between<User>(p => p.Name, new BetweenValues { Value1 = 1, Value2 = 10 }, true);
            string idsql = idFieldPredicate.GetSql(sqlGeneratorImpl, new Dictionary<string, object>());
            Assert.Equal("(`User`.`Name` NOT BETWEEN @Name_0 AND @Name_1)", idsql);

            IPropertyPredicate propertyPredicate = Predicates.Property<User, Role>(u => u.RoleId, Operator.Eq, r => r.Id, true);
            string propertysql = propertyPredicate.GetSql(sqlGeneratorImpl, new Dictionary<string, object>());
            Assert.Equal("(`User`.`RoleId` <> `Role`.`Id`)", propertysql);

            IExistsPredicate existsPredicate = Predicates.Exists<User>(nameFieldPredicate, true);
            string existssql = existsPredicate.GetSql(sqlGeneratorImpl, new Dictionary<string, object>());
            Assert.Equal("(NOT EXISTS (SELECT 1 FROM `User` WHERE (`User`.`Name` LIKE @Name_0)))", existssql);

            IList<IPredicate> predList = new List<IPredicate>();
            predList.Add(nameFieldPredicate);
            predList.Add(idFieldPredicate);
            IPredicateGroup predGroup1 = Predicates.Group(GroupOperator.And, predList.ToArray());

            IList<IPredicate> predList2 = new List<IPredicate>();
            predList2.Add(predGroup1);
            predList2.Add(existsPredicate);
            IPredicateGroup predGroup2 = Predicates.Group(GroupOperator.Or, predList2.ToArray());
            string groupsql = predGroup2.GetSql(sqlGeneratorImpl, new Dictionary<string, object>());
            string res = "(((`User`.`Name` LIKE @Name_0) AND (`User`.`Name` NOT BETWEEN @Name_1 AND @Name_2)) OR (NOT EXISTS (SELECT 1 FROM `User` WHERE (`User`.`Name` LIKE @Name_3))))";
            Assert.Equal(groupsql, res);
        }
    }
}
