using AutoMapper;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using ONE.Abp.Shared.Utils;
using ONE.Abp.DataPermission.Rules;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AutoMapper;

namespace ONE.Abp.DataPermission;

public class DataPermissionApplicationAutoMapperProfile : Profile
{
    public DataPermissionApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<UserDataRule, UserDataRuleDto>().Ignore(d=>d.DataRuleName).Ignore(d=>d.UserRuleName);
        CreateMap<UserRule, UserRuleDto>().ForMember(d => d.ConditionGroups, opt => opt.MapFrom(s => s.Condition.FromJson<List<ConditionGroupUnit>>(null)));
        CreateMap<UserRule, UserRuleMini>();

        CreateMap<DataRule, DataRuleDto>().ForMember(d => d.ConditionGroups, opt => opt.MapFrom(s => s.Condition.FromJson<List<ConditionGroupUnit>>(null)))
            .ForMember(d => d.HideDataTargetFields, opt => opt.MapFrom(s => MapListFromStr(s.HideDataTargetFields)))
            .ForMember(d => d.HideDataTargetFieldDisplayNames, opt => opt.MapFrom(s => MapListFromStr(s.HideDataTargetFieldDisplayNames)))
            .ForMember(d => d.DataOperations, opt => opt.MapFrom(s => RuleDataOperationHelper.FromRuleDataOperation(s.DataOperation)));


        CreateMap<DataRule, DataRuleMini>()
           .ForMember(d => d.HideDataTargetFields, opt => opt.MapFrom(s => MapListFromStr(s.HideDataTargetFields)))
            .ForMember(d => d.HideDataTargetFieldDisplayNames, opt => opt.MapFrom(s => MapListFromStr(s.HideDataTargetFieldDisplayNames)))
           .ForMember(d => d.DataOperations, opt => opt.MapFrom(s => RuleDataOperationHelper.FromRuleDataOperation(s.DataOperation)));

        CreateMap<DataTarget, DataTargetDto>();
        CreateMap<DataTargetField, DataTargetFieldDto>();
    }


    static IList<string> MapListFromStr(string str)
    {
        if(string.IsNullOrWhiteSpace(str)) return new List<string>();
        return str.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
    }
}
