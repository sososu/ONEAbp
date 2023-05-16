using JetBrains.Annotations;
using System;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.SysResource.SysApps
{
    public class SysAppDto:ExtensibleAuditedEntityDto<Guid>
    {
        public string AppName { get; set; }

        public string AppVersion { get;  set; }

        public string AppCode { get;  set; }

        [CanBeNull]
        public string AppSecret { get;  set; }

        [CanBeNull]
        public string AppUrl { get;  set; }

        [CanBeNull]
        public string Description { get; set; }

    }
}
