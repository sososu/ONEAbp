using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace ONE.Abp.Data.DataDics
{
    public abstract class EnumDataItemRegisterionBase : IEnumDataItemRegisterion, ITransientDependency
    {
        public abstract void Register(AbpEnumDicOption option);
    }
}
