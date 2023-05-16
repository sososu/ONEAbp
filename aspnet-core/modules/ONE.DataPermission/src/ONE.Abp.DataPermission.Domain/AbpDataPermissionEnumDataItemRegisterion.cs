using ONE.Abp.Data.DataDics;
using ONE.Abp.Shared;
using ONE.Abp.Shared.Rules;

namespace ONE.Abp.DataPermission
{
    public class AbpDataPermissionEnumDataItemRegisterion : EnumDataItemRegisterionBase
    {
        public override void Register(AbpEnumDicOption option)
        {
            option.Add<QueryCompare>("规则比较方式");
            option.Add<RuleDataOperation>("规则操作权限");
            option.Add<ConditionOperator>("规则运算符");
            option.Add<RuleType>("规则类型");
        }
    }
}
