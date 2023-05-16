using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataTargetFieldDto:EntityDto
    {
        public string DataTargetName { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }

        [CanBeNull]
        public string DisplayName { get; set; }

        [CanBeNull]
        public string Description { get; set; }
    }
}
