using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.Libra.DapperExtensions.PredicateConver
{
    public class KeyPredicateConvert
    {
        public static IPredicate GetKeyPredicate<T>(IClassMapper classMap, T entity) where T : class
        {
            var whereFields = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            if (!whereFields.Any())
            {
                throw new ArgumentException("At least one Key column must be defined.");
            }

            IList<IPredicate> predicates = (from field in whereFields
                                            select new FieldPredicate<T>
                                            {
                                                Not = false,
                                                Operator = Operator.Eq,
                                                PropertyName = field.Name,
                                                Value = field.PropertyInfo.GetValue(entity, null)
                                            }).Cast<IPredicate>().ToList();

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }
    }
}
