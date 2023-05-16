using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 数据对象信息
    /// 存储所有需要进行权限控制的数据对象信息，包括名称、类型、描述等字段
    /// </summary>
    public class DataTarget : AuditedAggregateRoot
    {
        /// <summary>
        /// 对象名
        /// </summary>
        public string Name { get; set; }

        [CanBeNull]
        public string DisplayName { get; set; }

        [CanBeNull]
        public string Description { get; set; }

        public List<DataTargetField> Fields { get;protected set; }

        protected DataTarget() { }
        public DataTarget(string name)
        {
            Check.NotNull(name, nameof(name));
            Name = name;
            Fields = new List<DataTargetField>();
        }

        public override object[] GetKeys()
        {
            return new object[] { Name };
        }

        public void AddField(DataTargetField field)
        {
            if (IsInFields(field.Name))
                return;
            Fields.Add(field);
        }

        public void RemoveField(string name)
        {
            if (!IsInFields(name))
                return;
            Fields.RemoveAll(f=>f.Name==name);
        }

        public bool IsInFields(string name)
        {
            return Fields.Any(f => f.Name == name);
        }
    }

}
