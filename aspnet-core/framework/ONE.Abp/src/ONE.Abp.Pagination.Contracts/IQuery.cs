using System;
using System.Linq.Expressions;

namespace ONE.Abp.Pagination.Contracts
{
    /// <summary>
    /// 定义查询参数
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <typeparam name="TEntity">要查询的实体类型</typeparam>
        Expression<Func<TEntity, bool>> GetFilter<TEntity>() where TEntity : class;

        /// <summary>
        /// 增加并条件
        /// </summary>
        void And<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;
    }
}
