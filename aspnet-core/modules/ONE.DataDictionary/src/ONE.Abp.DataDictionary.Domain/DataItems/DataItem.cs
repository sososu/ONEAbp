using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ONE.Abp.DataDictionary.DataItems
{
    /// <summary>
    /// 数据字典项
    /// </summary>
    public class DataItem : AggregateRoot<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get;protected set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; protected set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DataItemStatus Status { get;protected set; }


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

        public Guid? ParentId { get; set; }  

        protected DataItem() { }

        public DataItem(Guid id,string name,string code,int? value=null,Guid? parentId=null)
        {
            Id = id;
            SetName(name);
            SetCode(code);
            if(value.HasValue) { SetValue(value.Value); }
            if(parentId.HasValue) { SetParentId(parentId.Value); }
        }

        public void SetName(string name)
        {
           Name=Check.NotNull(name, "name");    
        }

        public void SetCode(string code)
        {
            Code = Check.NotNull(code, "code");
        }

        public void SetStatus(DataItemStatus status)
        {
            Status = status;
        }

        public void SetValue(int value)
        {
            Value= Check.NotNull(value, "value");
        }

        public void SetParentId(Guid parentId)
        {
            ParentId= Check.NotNull(parentId, "parentId");
        }
    }
}
