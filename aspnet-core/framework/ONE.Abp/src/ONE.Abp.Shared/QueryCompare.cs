using System.ComponentModel.DataAnnotations;

namespace  ONE.Abp.Shared
{
    /// <summary>
    /// 查询比较方式
    /// </summary>
    public enum QueryCompare
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Display(Name = "等于")]
        Equal,

        /// <summary>
        /// 不等于
        /// </summary>
        [Display(Name = "不等于")]
        NotEqual,

        /// <summary>
        /// 模糊匹配
        /// </summary>
        [Display(Name = "模糊匹配")]
        Like,

        /// <summary>
        /// 不包含模糊匹配
        /// </summary>
        [Display(Name = "不包含模糊匹配")]
        NotLike,

        /// <summary>
        /// 以...开头
        /// </summary>
        [Display(Name = "以...开头")]
        StartWidth,

        /// <summary>
        /// 以...结尾
        /// </summary>
        [Display(Name = "以...结尾")]
        EndsWith,

        /// <summary>
        /// 小于
        /// </summary>
        [Display(Name = "小于")]
        LessThan,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Display(Name = "小于等于")]
        LessThanOrEqual,

        /// <summary>
        /// 大于
        /// </summary>
        [Display(Name = "大于")]
        GreaterThan,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Display(Name = "大于等于")]
        GreaterThanOrEqual,

        /// <summary>
        /// 在...之间(大于等于起始，小于结束)，属性必须是一个集合（或逗号分隔的字符串），取第一和最后一个值。
        /// 属性必须是数组或集合类型， 例如：
        /// [Query(QueryCompare.Between)]
        /// public DateTime[] CreationDate { get; set; }
        /// </summary>
        [Display(Name = "在...之间")]
        Between,

        /// <summary>
        /// 大于等于起始，小于结束，属性必须是一个集合（或逗号分隔的字符串），取第一和最后一个值。
        /// 属性必须是数组或集合类型， 例如：
        /// [Query(QueryCompare.GreaterEqualAndLess)]
        /// public DateTime[] CreationDate { get; set; }
        /// </summary>
        [Display(Name = "大于等于起始，小于结束")]
        GreaterEqualAndLess,

        /// <summary>
        /// 大于等于起始，小于等于结束，属性必须是一个集合（或逗号分隔的字符串），取第一和最后一个值。
        /// 属性必须是数组或集合类型， 例如：
        /// [Query(QueryCompare.GreaterEqualAndLessEqual)]
        /// public DateTime[] CreationDate { get; set; }
        /// </summary>
        [Display(Name = "大于等于起始，小于等于结束")]
        GreaterEqualAndLessEqual,

        /// <summary>
        /// 包含，属性必须是一个集合（或逗号分隔的字符串）
        /// 属性必须是数组或集合类型， 例如：
        /// [Query(QueryCompare.Include)]
        /// public string[] Ids { get; set; }
        /// </summary>
        [Display(Name = "包含")]
        Include,

        /// <summary>
        /// 不包含，属性必须是一个集合（或逗号分隔的字符串）
        /// </summary>
        [Display(Name = "不包含")]
        NotInclude,

        /// <summary>
        /// 为空或不为空，可以为 bool类型，或可空类型。
        /// </summary>
        [Display(Name = "为空或不为空")]
        IsNull,

        /// <summary>
        /// 是否包含指定枚举
        /// </summary>
        [Display(Name = "是否包含指定枚举")]
        HasFlag,
    }
}
