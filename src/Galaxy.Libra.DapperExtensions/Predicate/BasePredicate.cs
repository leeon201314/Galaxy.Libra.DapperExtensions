using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.Libra.DapperExtensions.Predicate
{
    public interface IPredicate
    {
        string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters);
    }

    public interface IBasePredicate : IPredicate
    {
        string PropertyName { get; set; }
    }

    public abstract class BasePredicate : IBasePredicate
    {
        public abstract string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters);
        public string PropertyName { get; set; }

        protected virtual string GetColumnName(Type entityType, ISqlGenerator sqlGenerator, string propertyName)
        {
            IClassMapper map = sqlGenerator.Configuration.GetMap(entityType);
            if (map == null)
                throw new NullReferenceException($"{entityType}找不到ClassMap映射文件");

            IPropertyMap propertyMap = map.Properties.SingleOrDefault(p => p.Name == propertyName);
            if (propertyMap == null)
                throw new NullReferenceException($"{entityType}找不到属性{propertyName}");

            return sqlGenerator.GetColumnName(map, propertyMap, false);
        }
    }
}
