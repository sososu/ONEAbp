using JetBrains.Annotations;
using System;

namespace ONE.Abp.Shared.Rules
{
    public class DataTargetInfo
    {
        public Type DataTargetType { get; set; }

        public bool NeedUpdateShadowProperty { get;set;}

        [CanBeNull]
        public string DisplayName { get; set; }

        [CanBeNull]
        public string Description { get; set; }
    }
}
