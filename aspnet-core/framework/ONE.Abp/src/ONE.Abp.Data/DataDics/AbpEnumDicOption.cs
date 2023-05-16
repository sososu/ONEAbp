using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ONE.Abp.Data.DataDics
{
    public class AbpEnumDicOption
    {
        internal List<EnumDicInfo> EnumDicInfos { get; }

        public void Add<T>(string name) where T : Enum
        {
            if (EnumDicInfos.Exists(q => q.EnmuType == typeof(T)))
                return;
            if (EnumDicInfos.Exists(q => q.Name == name))
                throw new Exception("Duplicate names exist in the data dictionary");

            EnumDicInfos.Add(new EnumDicInfo { EnmuType = typeof(T), Name = name });
        }

        public void Remove<T>() where T : Enum
        {
            EnumDicInfos.RemoveAll(q => q.EnmuType == typeof(T));
        }

        public IReadOnlyCollection<EnumDicInfo> GetInfos()
        {
            return EnumDicInfos.ToImmutableList();
        }

        public AbpEnumDicOption() { EnumDicInfos = new List<EnumDicInfo>(); }

    }

    public class EnumDicInfo
    {
        public Type EnmuType { get; set; }

        public string Name { get; set; }
    }
}
