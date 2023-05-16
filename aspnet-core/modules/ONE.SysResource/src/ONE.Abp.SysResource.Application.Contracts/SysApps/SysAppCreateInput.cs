using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace ONE.Abp.SysResource.SysApps
{
    public class SysAppCreateInput:ExtensibleObject
    {
        [Required]
        [MaxLength(64)]
        public string AppName { get;  set; }

        [Required]
        [MaxLength(64)]
        public string AppVersion { get;  set; }

        [Required]
        [MaxLength(64)]
        public string AppCode { get;  set; }

        [CanBeNull]
        public string AppSecret { get;  set; }

        [CanBeNull]
        public string AppUrl { get;  set; }

        [CanBeNull]
        public string Description { get;  set; }

    }

    public class SysAppUpdateInput: SysAppCreateInput
    {
       
    }
}
