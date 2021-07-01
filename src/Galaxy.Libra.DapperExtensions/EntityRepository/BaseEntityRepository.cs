using Dapper;
using Galaxy.Libra.DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Galaxy.Libra.DapperExtensions.EntityRepository
{
    public abstract class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : class
    {
        protected IDbConnection curDbConnection = null;

        #region 增删改

        public virtual dynamic Add(T entity) => Execute(() => curDbConnection.Insert<T>(entity));

        public virtual bool Update(T entity) => Execute(() => curDbConnection.Update(entity));

        public virtual bool Delete(Expression<Func<T, bool>> expression) => Execute(() => curDbConnection.Delete(expression));

        #endregion

        /// <summary>
        /// 获取,参数为null时，则获取整个表数据
        /// </summary>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }</param>
        public virtual List<T> GetList(object predicate = null) => Execute(() => curDbConnection.GetList<T>(predicate).AsList<T>());

        public virtual List<T> GetList(Expression<Func<T, bool>> expression) => Execute(() => curDbConnection.GetList<T>(expression).AsList<T>());

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">每页记录数</param>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }, 更多的请使用Predicates</param>
        public virtual List<T> GetPage(int page, int resultsPerPage, IList<ISort> sort, object predicate = null)
            => Execute(() => curDbConnection.GetPage<T>(predicate, sort, page, resultsPerPage).AsList<T>());

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">每页记录数</param>
        public virtual List<T> GetPage(int page, int resultsPerPage, IList<ISort> sort, Expression<Func<T, bool>> expression)
            => Execute(() => curDbConnection.GetPage<T>(expression, sort, page, resultsPerPage).AsList<T>());

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }</param>
        /// <returns></returns>
        public virtual int Count(object predicate = null) => Execute(() => curDbConnection.Count<T>(predicate));

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }</param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> expression) => Execute(() => curDbConnection.Count<T>(expression));

        public dynamic Max(Expression<Func<T, object>> maxExp, Expression<Func<T, bool>> expression)
        {
            return Execute(() => curDbConnection.Max<T>(maxExp, expression));
        }

        public dynamic Max(Expression<Func<T, object>> maxExp, object predicate = null)
        {
            return Execute(() => curDbConnection.Max<T>(maxExp, predicate));
        }

        public dynamic Sum(Expression<Func<T, object>> maxExp, Expression<Func<T, bool>> expression)
        {
            return Execute(() => curDbConnection.Sum<T>(maxExp, expression));
        }

        public dynamic Sum(Expression<Func<T, object>> maxExp, object predicate = null)
        {
            return Execute(() => curDbConnection.Sum<T>(maxExp, predicate));
        }

        private R Execute<R>(Func<R> fun)
        {
            if (curDbConnection != null && fun != null)
            {
                using (curDbConnection)
                {
                    if (curDbConnection.State != ConnectionState.Open)
                        curDbConnection.Open();

                    R res = fun();

                    curDbConnection.Close();
                    return res;
                }
            }

            return default(R);
        }

        private void ExecuteNoValue(Action action)
        {
            if (curDbConnection != null && action != null)
            {
                using (curDbConnection)
                {
                    if (curDbConnection.State != ConnectionState.Open)
                        curDbConnection.Open();

                    action();

                    curDbConnection.Close();
                }
            }
        }

        
    }
}
