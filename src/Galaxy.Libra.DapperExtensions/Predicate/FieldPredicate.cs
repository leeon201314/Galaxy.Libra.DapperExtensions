using Galaxy.Libra.DapperExtensions.Sql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.Libra.DapperExtensions.Predicate
{
    public interface IFieldPredicate : IComparePredicate
    {
        object Value { get; set; }
    }

    public class FieldPredicate<T> : ComparePredicate, IFieldPredicate
        where T : class
    {
        public object Value { get; set; }

        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            if (Value == null)
                return $"({columnName} IS {NotStr}NULL)";

            if (Value is IEnumerable && !(Value is string))
            {
                if (Operator != Operator.Eq)
                    throw new ArgumentException("Operator must be set to Eq for Enumerable types");

                List<string> @params = new List<string>();
                foreach (var value in (IEnumerable)Value)
                {
                    string valueParameterName = parameters.SetParameterName(this.PropertyName, value, sqlGenerator.Configuration.Dialect.ParameterPrefix);
                    @params.Add(valueParameterName);
                }

                string paramStrings = @params.Aggregate(new StringBuilder(), (sb, s) => sb.Append((sb.Length != 0 ? ", " : string.Empty) + s), sb => sb.ToString());
                return $"({columnName} {NotStr}IN ({paramStrings}))";
            }

            string parameterName = parameters.SetParameterName(this.PropertyName, this.Value, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            string operatorString = GetOperatorString();
            return $"({columnName} {operatorString} {parameterName})";
        }
    }
}
