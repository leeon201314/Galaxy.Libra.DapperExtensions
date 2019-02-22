using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Sql;
using System;
using System.Collections.Generic;

namespace Galaxy.Libra.DapperExtensions.Predicate
{
    public interface IExistsPredicate : IPredicate
    {
        IPredicate Predicate { get; set; }
        bool Not { get; set; }
    }

    public class ExistsPredicate<TSub> : IExistsPredicate
        where TSub : class
    {
        public IPredicate Predicate { get; set; }
        public bool Not { get; set; }

        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            IClassMapper mapSub = GetClassMapper(typeof(TSub), sqlGenerator.Configuration);
            string notStr = Not ? "NOT " : string.Empty;
            string tableName = sqlGenerator.GetTableName(mapSub);
            string predicateSql = Predicate.GetSql(sqlGenerator, parameters);
            return $"({notStr}EXISTS (SELECT 1 FROM {tableName} WHERE {predicateSql}))";
        }

        protected virtual IClassMapper GetClassMapper(Type type, IDapperExtensionsConfiguration configuration)
        {
            IClassMapper map = configuration.GetMap(type);

            if (map == null)
                throw new NullReferenceException($"{type}找不到ClassMap映射文件");

            return map;
        }
    }
}
