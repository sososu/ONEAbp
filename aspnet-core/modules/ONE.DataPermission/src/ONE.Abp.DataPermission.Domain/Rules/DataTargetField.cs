using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 数据对象字段信息
    /// </summary>
    public class DataTargetField : Entity
    {

        public string DataTargetName { get; protected set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get;protected set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; protected set; }

        [CanBeNull]
        public string DisplayName { get; set; }

        [CanBeNull]
        public string Description { get; set; }

        protected DataTargetField() { }
        public DataTargetField(string dataTargetName, string name, string type)
        {
            Check.NotNull(dataTargetName, nameof(dataTargetName));
            Check.NotNull(name, nameof(name));
            Check.NotNull(type, nameof(type));
            DataTargetName= dataTargetName;
            Name = name;
            Type = type;
        }

        public virtual void SetType(string type)
        {
            Check.NotNull(type, nameof(type));
            Type = type;
        }

        public override object[] GetKeys()
        {
            return new object[] { DataTargetName,Name };
        }

       
    }
}
