using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataTargetDto : AuditedEntityDto
    {
        /// <summary>
        /// 对象名
        /// </summary>
        public string Name { get; set; }

        [CanBeNull]
        public string DisplayName { get; set; }

        [CanBeNull]
        public string Description { get; set; }

        public List<DataTargetFieldDto> Fields { get; set; }
    }
}
