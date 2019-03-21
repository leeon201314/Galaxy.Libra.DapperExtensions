using Galaxy.Libra.DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Galaxy.Libra.DapperExtensions.PredicateConver
{
    public class ExpressionPredicateConvert
    {
        /// <summary>
        /// 将表达式转换为Predicate
        /// </summary>
        public static IPredicate GetExpressionPredicate<T>(Expression<Func<T, bool>> expression) where T : class
        {
            Expression expr = expression.Body;
            return ConvertToPredicate<T>(expr);
        }

        private static IPredicate ConvertToPredicate<T>(Expression expr) where T : class
        {
            if (expr.NodeType == ExpressionType.OrElse || expr.NodeType == ExpressionType.AndAlso)
            {
                BinaryExpression binExpr = expr as BinaryExpression;
                IList<IPredicate> predList = new List<IPredicate> { ConvertToPredicate<T>(binExpr.Left), ConvertToPredicate<T>(binExpr.Right) };
                GroupOperator op = expr.NodeType == ExpressionType.OrElse ? GroupOperator.Or : GroupOperator.And;
                return Predicates.Group(op, predList.ToArray());
            }
            else if (expr.NodeType == ExpressionType.Equal || expr.NodeType == ExpressionType.NotEqual
                || expr.NodeType == ExpressionType.GreaterThan || expr.NodeType == ExpressionType.GreaterThanOrEqual
                || expr.NodeType == ExpressionType.LessThan || expr.NodeType == ExpressionType.LessThanOrEqual)
            {
                return ConvertBaseExpression<T>(expr as BinaryExpression);
            }
            else if (expr.NodeType == ExpressionType.Call)
            {
                return ConverCallExpression<T>(expr);
            }
            else if (expr.NodeType == ExpressionType.Not)
            {
                return ConverNotInExpression<T>(expr);
            }

            return null;
        }

        private static IPredicate ConvertBaseExpression<T>(Expression expr) where T : class
        {
            BinaryExpression binExpr = expr as BinaryExpression;
            MemberExpression menLeftExpr = binExpr.Left as MemberExpression;
            ConstantExpression conRightExpr = binExpr.Right as ConstantExpression;

            if (menLeftExpr != null)
            {
                Operator op = Operator.Eq;
                bool not = false;

                switch (binExpr.NodeType)
                {
                    case ExpressionType.Equal:
                        op = Operator.Eq;
                        break;
                    case ExpressionType.NotEqual:
                        op = Operator.Eq;
                        not = true;
                        break;
                    case ExpressionType.LessThan:
                        op = Operator.Lt;
                        break;
                    case ExpressionType.GreaterThan:
                        op = Operator.Gt;
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        op = Operator.Ge;
                        break;
                    case ExpressionType.LessThanOrEqual:
                        op = Operator.Le;
                        break;
                    default:
                        throw new Exception($"未实现该操作符逻辑{binExpr.NodeType}");
                }

                return new FieldPredicate<T>
                {
                    PropertyName = menLeftExpr.Member.Name,
                    Operator = op,
                    Not = not,
                    Value = GetExpressionValue(binExpr.Right)
                };
            }

            return null;
        }

        private static object GetExpressionValue(Expression expr)
        {
            ConstantExpression con = expr as ConstantExpression;
            if (con != null)
                return con.Value;

            MemberExpression mem = expr as MemberExpression;
            if (mem != null)
            {
                if (mem.Type == typeof(int))
                    return Expression.Lambda<Func<int>>(mem).Compile()();
                else if (mem.Type == typeof(double))
                    return Expression.Lambda<Func<double>>(mem).Compile()();
                else if (mem.Type == typeof(float))
                    return Expression.Lambda<Func<float>>(mem).Compile()();
                else if (mem.Type == typeof(decimal))
                    return Expression.Lambda<Func<decimal>>(mem).Compile()();
                else if (mem.Type == typeof(long))
                    return Expression.Lambda<Func<long>>(mem).Compile()();
                else if (mem.Type == typeof(string))
                    return Expression.Lambda<Func<string>>(mem).Compile()();
                else
                    return Expression.Lambda<Func<object>>(mem).Compile()();
            }

            return null;
        }

        private static IPredicate ConverCallExpression<T>(Expression expr) where T : class
        {
            MethodCallExpression methExpr = expr as MethodCallExpression;
            MemberExpression menExpr = methExpr.Object as MemberExpression;

            //判断是否是列表，如果是列表则进行in条件构建
            if (menExpr.Type.IsGenericType == true || menExpr.Type.IsArray == true)
            {
                string propertyName = (methExpr.Arguments[0] as MemberExpression).Member.Name;
                object value = Expression.Lambda<Func<object>>(menExpr).Compile()();

                return new FieldPredicate<T>
                {
                    PropertyName = propertyName,
                    Operator = Operator.Eq,
                    Value = value
                };
            }
            else
            {
                string propertyName = menExpr.Member.Name;
                string methName = methExpr.Method.Name;
                string paramStr = (methExpr.Arguments[0] as ConstantExpression).Value.ToString();

                switch (methName)
                {
                    case "Contains":
                        paramStr = $"%{paramStr}%";
                        break;
                    case "StartsWith":
                        paramStr = $"{paramStr}%";
                        break;
                    case "EndsWith":
                        paramStr = $"%{paramStr}";
                        break;
                    default:
                        throw new Exception($"未能解析该方法{methName}");
                }

                return new FieldPredicate<T>
                {
                    PropertyName = propertyName,
                    Operator = Operator.Like,
                    Value = paramStr
                };
            }
        }

        private static IPredicate ConverNotInExpression<T>(Expression expr) where T : class
        {
            UnaryExpression tempExpr = expr as UnaryExpression;
            IPredicate predicate = ConverCallExpression<T>(tempExpr.Operand);
            (predicate as FieldPredicate<T>).Not = true;
            return predicate;
        }
    }
}
