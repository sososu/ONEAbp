using ONE.Abp.Data.DataDics;
using Volo.Abp.Identity;

namespace ONE.Admin
{
    public class ONEEnumDataItemRegisterion : EnumDataItemRegisterionBase
    {
        public override void Register(AbpEnumDicOption option)
        {
            option.Add<Sex>("性别");
        }
    }

}
