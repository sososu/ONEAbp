using ONE.Abp.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.Shared.Rules
{
    /// <summary>
    /// 条件
    /// </summary>
    public class Condition
    {
        /// <summary> 
        /// 值  如果是集合用","分割
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }


        /// <summary>
        /// 比较方式
        /// </summary>
        public QueryCompare Compare { get; set; }
    }

    #region 单链表


    ///// <summary>
    ///// 条件链
    ///// </summary>
    //public class ConditionChain
    //{
    //    public Condition Condition { get; set; }

    //    public ConditionChain Next { get; set; }

    //    public ConditionOperator? ConditionOperator { get; set; }
    //}


    ///// <summary>
    ///// 条件组
    ///// </summary>
    //public class ConditionGroup
    //{
    //    /// <summary>
    //    /// 组名称
    //    /// </summary>
    //    public string Name { get; set; }

    //    public ConditionChain Condition { get; set; }
    //}


    ///// <summary>
    ///// 条件组链
    ///// </summary>
    //public class ConditionGroupChain
    //{
    //    public ConditionGroup Group { get; set; }

    //    public ConditionGroupChain Next { get; set; }

    //    public ConditionOperator? ConditionOperator { get; set; }
    //}

    #endregion

    /// <summary>
    /// 条件组单元
    /// </summary>
    public class ConditionGroupUnit
    {
        /// <summary>
        /// 组名称
        /// </summary>
        public string Name { get; set; }

        public List<ConditionUnit> ConditionUnits { get; set; }

        public ConditionOperator? ConditionOperator { get; set; }
    }


    /// <summary>
    /// 条件单元
    /// </summary>
    public class ConditionUnit
    {
        public Condition Condition { get; set; }

        public ConditionOperator? ConditionOperator { get; set; }
    }

    /// <summary>
    /// 条件操作类型
    /// </summary>
    public enum ConditionOperator
    {
        /// <summary>
        /// 并且
        /// </summary>
        [Display(Name = "并且")]
        And = 0,

        /// <summary>
        /// 或者
        /// </summary>
        [Display(Name = "或者")]
        Or = 1,
    }
}
