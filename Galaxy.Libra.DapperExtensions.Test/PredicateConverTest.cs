using Galaxy.Libra.DapperExtensions.Predicate;
using Galaxy.Libra.DapperExtensions.PredicateConver;
using Galaxy.Libra.DapperExtensions.Test.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Galaxy.Libra.DapperExtensions.Test
{
    public class PredicateConverTest
    {
        [Fact]
        public void TestBase()
        {
            IPredicate p1 = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => u.Id != 1);
            FieldPredicate<User> f1 = p1 as FieldPredicate<User>;
            Assert.True(f1.Operator == Operator.Eq);
            Assert.True(f1.Not);

            IPredicate pGe = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => u.Id >= 1);
            FieldPredicate<User> fGe = pGe as FieldPredicate<User>;
            Assert.True(fGe.Operator == Operator.Ge);
        }

        [Fact]
        public void TestLike()
        {
            IPredicate p1 = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => u.Name.Contains("1"));
            FieldPredicate<User> f1 = p1 as FieldPredicate<User>;
            Assert.True(f1.Operator == Operator.Like);
            Assert.True(f1.Value.ToString() == "%1%");

            IPredicate p2 = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => u.Name.StartsWith("1"));
            FieldPredicate<User> f2 = p2 as FieldPredicate<User>;
            Assert.True(f2.Operator == Operator.Like);
            Assert.True(f2.Value.ToString() == "1%");

            IPredicate p3 = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => u.Name.EndsWith("1"));
            FieldPredicate<User> f3 = p3 as FieldPredicate<User>;
            Assert.True(f3.Operator == Operator.Like);
            Assert.True(f3.Value.ToString() == "%1");
        }

        [Fact]
        public void TestIn()
        {
            List<string> valueList = new List<string>() { "1", "2", "3" };
            IPredicate p = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => valueList.Contains(u.Name));
            FieldPredicate<User> f = p as FieldPredicate<User>;
            Assert.True(f.Operator == Operator.Eq);
            Assert.True((f.Value as List<string>).Count == 3);
        }

        [Fact]
        public void TestGroup()
        {
            IPredicate p = ExpressionPredicateConver.GetExpressionPredicate<User>(new UserMapper(), u => u.Id == 1 || (u.Id == 2 && u.Name.Contains("1")));
            FieldPredicate<User> f = (p as IPredicateGroup).Predicates[0] as FieldPredicate<User>;
            Assert.True(System.Convert.ToInt32(f.Value) == 1);
            Assert.True((p as IPredicateGroup).Operator == GroupOperator.Or);
            Expression<Func<User, bool>> expr = u => u.Id == 1 || (u.Id == 2 && u.Name.Contains("1"));
            GetPredicate(expr);
        }

        protected IPredicate GetPredicate(object predicate)
        {
            IPredicate wherePredicate = predicate as IPredicate;
            if (wherePredicate == null && predicate != null)
            {
                
            }

            return wherePredicate;
        }
    }
}
