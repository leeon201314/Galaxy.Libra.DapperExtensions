using Galaxy.Libra.DapperExtensions.Sql;
using System.Collections.Generic;

namespace Galaxy.Libra.DapperExtensions.Predicate
{
    public interface IPropertyPredicate : IComparePredicate
    {
        string PropertyName2 { get; set; }
    }

    public class PropertyPredicate<T, T2> : ComparePredicate, IPropertyPredicate
        where T : class
        where T2 : class
    {
        public string PropertyName2 { get; set; }

        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            string columnName2 = GetColumnName(typeof(T2), sqlGenerator, PropertyName2);
            string operatorString = GetOperatorString();
            return $"({columnName} {operatorString} {columnName2})";
        }
    }
}
