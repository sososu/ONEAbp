using ONE.Abp.Data.DataDics;
using System.ComponentModel.DataAnnotations;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameEnumDataItemRegisterion : EnumDataItemRegisterionBase
    {
        public override void Register(AbpEnumDicOption option)
        {
            //option.Add<Sex>("性别");
            option.Add<Test>("测试");
        }
    }

    public enum Test
    {
        [Display(Name ="无")]
        None = 0,

        [Display(Name = "有")]
        Has = 1,
    }
}
