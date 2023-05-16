using System;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.DataDictionary.DataItems
{
    public class DataCategoryCreateInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DataItemStatus Status { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }
    }

    public  class DataCategoryUpdate : DataCategoryCreateInput
    {
       
    }



    public class DataItemCreateInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DataItemStatus Status { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Required]
        public Guid DataCategoryId { get; set; }

    }

    public class DataItemCreateUpdate : DataItemCreateInput
    {
        
    }
}
