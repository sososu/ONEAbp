using AutoMapper;
using ONE.Abp.Shared.Rules;
using ONE.Abp.Shared.Utils;
using ONE.Abp.DataPermission.Rules;
using System.Collections.Generic;
using Volo.Abp.AutoMapper;

namespace ONE.Abp.DataPermission;

public class DataPermissionDomainAutoMapperProfile : Profile
{
    public DataPermissionDomainAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<UserDataRule, UserDataRuleResult>().Ignore(d => d.DataRuleName).Ignore(d => d.UserRuleName);
        CreateMap<UserRule, UserRuleResult>().ForMember(d => d.Conditions, opt => opt.MapFrom(s => s.Condition.FromJson<List<ConditionGroupUnit>>(null)));
        CreateMap<DataRule, DataRuleResult>().ForMember(d => d.Conditions, opt => opt.MapFrom(s => s.Condition.FromJson<List<ConditionGroupUnit>>(null)));
    }
}
