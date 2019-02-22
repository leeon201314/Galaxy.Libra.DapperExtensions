namespace Galaxy.Libra.DapperExtensions.Predicate
{
    public interface IComparePredicate : IBasePredicate
    {
        Operator Operator { get; set; }
        bool Not { get; set; }
    }

    public abstract class ComparePredicate : BasePredicate
    {
        public Operator Operator { get; set; }
        public bool Not { get; set; }
        public string NotStr
        {
            get
            {
                return Not ? "NOT " : string.Empty;
            }
        }

        public virtual string GetOperatorString()
        {
            switch (Operator)
            {
                case Operator.Gt:
                    return Not ? "<=" : ">";
                case Operator.Ge:
                    return Not ? "<" : ">=";
                case Operator.Lt:
                    return Not ? ">=" : "<";
                case Operator.Le:
                    return Not ? ">" : "<=";
                case Operator.Like:
                    return Not ? "NOT LIKE" : "LIKE";
                default:
                    return Not ? "<>" : "=";
            }
        }
    }

    /// <summary>
    /// 操作符。
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// Equal to
        /// </summary>
        Eq,

        /// <summary>
        /// Greater than
        /// </summary>
        Gt,

        /// <summary>
        /// Greater than or equal to
        /// </summary>
        Ge,

        /// <summary>
        /// Less than
        /// </summary>
        Lt,

        /// <summary>
        /// Less than or equal to
        /// </summary>
        Le,

        /// <summary>
        /// Like (You can use % in the value to do wilcard searching)
        /// </summary>
        Like
    }
}
