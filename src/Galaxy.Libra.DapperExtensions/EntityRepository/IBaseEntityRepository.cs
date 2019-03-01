using Galaxy.Libra.DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Galaxy.Libra.DapperExtensions.EntityRepository
{
    public interface IBaseEntityRepository<T> where T : class
    {
        #region 增删改

        dynamic Add(T entity);

        bool Update(T entity);

        bool Delete(Expression<Func<T, bool>> expression);

        #endregion

        /// <summary>
        /// 获得,全部参数为null
        /// </summary>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }, 更多的请使用Predicates</param>
        /// <returns></returns>
        List<T> GetList(object predicate = null);

        List<T> GetList(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">每页记录数</param>
        /// <param name="buffered">是否缓存</param>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }, 更多的请使用Predicates</param>
        /// <returns></returns>
        List<T> GetPage(int page, int resultsPerPage, IList<ISort> sort, object predicate = null);

        List<T> GetPage(int page, int resultsPerPage, IList<ISort> sort, Expression<Func<T, bool>> expression);

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="predicate">参数示例，如：new { LastName = "Bar", FirstName = "Foo" }, 更多的请使用Predicates</param>
        /// <returns></returns>
        int Count(object predicate = null);

        int Count(Expression<Func<T, bool>> expression);
    }
}
