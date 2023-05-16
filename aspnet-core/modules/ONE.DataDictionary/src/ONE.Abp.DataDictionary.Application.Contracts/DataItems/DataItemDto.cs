using ONE.Abp.Shared.Utils;
using System;
using Volo.Abp.Application.Dtos;
namespace ONE.Abp.DataDictionary.DataItems
{
    public class DataItemDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DataItemStatus Status { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public string StatusStr => Status.DisplayName();

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public DataItemSource Source { get; set; }


        /// <summary>
        /// 来源
        /// </summary>
        public string SourceStr => Source.DisplayName();

        public Guid? ParentId { get; set; }
    }
}
