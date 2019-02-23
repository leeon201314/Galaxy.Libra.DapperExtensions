using Galaxy.Libra.DapperExtensions.Mapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using Galaxy.Libra.DapperExtensions.PredicateConver;
using Galaxy.Libra.DapperExtensions.Sql;

namespace Galaxy.Libra.DapperExtensions.DapperImpl
{
    public partial class DapperImplementor : IDapperImplementor
    {
        public DapperImplementor(ISqlGenerator sqlGenerator)
        {
            SqlGenerator = sqlGenerator;
        }

        public ISqlGenerator SqlGenerator { get; private set; }
            
        protected IPredicate GetPredicate(IClassMapper classMap, object predicate)
        {
            IPredicate wherePredicate = predicate as IPredicate;
            if (wherePredicate == null && predicate != null)
            {
                wherePredicate = EntityPredicateConvert.GetEntityPredicate(classMap, predicate);
            }

            return wherePredicate;
        }
    }
}
