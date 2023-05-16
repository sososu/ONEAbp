using  ONE.Abp.Shared;
using System;
using System.Linq;

namespace ONE.Abp.Pagination.Contracts.Attributes
{
    /// <summary>
    /// 查询字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class QueryAttribute : Attribute
    {
        /// <summary>
        /// 比较方式
        /// </summary>
        public QueryCompare Compare { get; set; }

        /// <summary>
        /// 对应属性路径
        /// </summary>
        public string[] PropertyPath { get; set; }

        /// <summary>
        /// 或条件组
        /// </summary>
        public string OrGroup { get; set; }

        /// <summary>
        /// 查询字段
        /// </summary>
        public QueryAttribute(params string[] propertyPath)
        {
            propertyPath = propertyPath?.SelectMany(p => p.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            if (propertyPath?.Length > 0)
                PropertyPath = propertyPath;
        }

        /// <summary>
        /// 查询字段
        /// </summary>
        public QueryAttribute(QueryCompare compare, params string[] propertyPath)
        {
            propertyPath = propertyPath?.SelectMany(p => p.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            if (propertyPath?.Length > 0)
                PropertyPath = propertyPath;

            Compare = compare;
        }
    }
}
