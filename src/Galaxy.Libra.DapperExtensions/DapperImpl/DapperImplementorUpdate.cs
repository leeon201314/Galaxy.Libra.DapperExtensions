using Dapper;
using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using Galaxy.Libra.DapperExtensions.PredicateConver;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Galaxy.Libra.DapperExtensions.DapperImpl
{
    public partial class DapperImplementor
    {
        public bool Update<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = KeyPredicateConvert.GetKeyPredicate<T>(classMap, entity);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Update(classMap, predicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }
    }
}
