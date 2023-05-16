using System;
using System.Linq;
using System.Linq.Expressions;

namespace ONE.Abp.Pagination.Contracts.Dtos
{
    /// <summary>
    /// 定义查询参数
    /// </summary>
    public class Query : IQuery
    {
        /// <summary>
        /// 指定查询条件
        /// </summary>
        protected LambdaExpression Filter { get; set; }

        /// <summary>
        /// 并且
        /// </summary>
        public virtual void And<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            Filter = (Filter as Expression<Func<TEntity, bool>>).And(filter);
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <typeparam name="TEntity">要查询的实体类型</typeparam>
        public Expression<Func<TEntity, bool>> GetFilter<TEntity>() where TEntity : class
        {
            return (Filter as Expression<Func<TEntity, bool>>).And(this.GetQueryExpression<TEntity>());
        }
    }
}
