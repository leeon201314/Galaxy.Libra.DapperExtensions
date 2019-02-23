using Dapper;
using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Galaxy.Libra.DapperExtensions.DapperImpl
{
    public partial class DapperImplementor
    {
        public int Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Count(classMap, wherePredicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return (int)connection.Query(sql, dynamicParameters, transaction, false, commandTimeout, CommandType.Text).Single().Total;
        }
    }
}
