using Dapper;
using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using Galaxy.Libra.DapperExtensions.PredicateConver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Galaxy.Libra.DapperExtensions.DapperImpl
{
    public partial class DapperImplementor
    {
        public dynamic Sum<T>(IDbConnection connection, Expression<Func<T, object>> expMax, Expression<Func<T, bool>> expression, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = ExpressionPredicateConvert.GetExpressionPredicate(expression);
            string propertyName = ExpressionPredicateConvert.PropertyName<T>(expMax);
            return Sum<T>(connection, classMap, propertyName, wherePredicate, transaction, commandTimeout);
        }


        public dynamic Sum<T>(IDbConnection connection, Expression<Func<T, object>> expMax, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            string propertyName = ExpressionPredicateConvert.PropertyName<T>(expMax);
            return Sum<T>(connection, classMap, propertyName, wherePredicate, transaction, commandTimeout);
        }

        protected dynamic Sum<T>(IDbConnection connection, IClassMapper classMap, string maxProperty, IPredicate predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Sum(classMap, predicate, parameters, maxProperty);
            System.Console.WriteLine(sql);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return connection.Query(sql, dynamicParameters, transaction, false, commandTimeout, CommandType.Text).Single().Sum;
        }
    }
}
