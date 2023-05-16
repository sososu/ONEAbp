using ONE.Abp.Shared;
using ONE.Abp.Shared.Rules;
using Volo.Abp.Collections;
using Volo.Abp.Domain.Entities;

namespace ONE.Abp.Data.Rules
{
    public class AbpRuleOptions
    {
        //public UserRuleExtraFieldNameManager UserRuleExtraFieldNameManager { get; }
        //public DataRulePredefineFieldValueManager DataRulePredefineFieldValueManager { get; }

        public RuleExtraFieldManager RuleExtraFieldManager { get; }

        public DataTargetOption DataTargetOption { get; }   
     
        /// <summary>
        /// If no rules are matched, value is true the system permits Otherwise not released 
        /// Default value is true
        /// </summary>
        public bool IsReleasedIfNoRulesAreMatched { get; set; } = true;

        /// <summary>
        /// If no rules are matched
        ///  Default value is Query|Delete|Edit
        /// </summary>
        public RuleDataOperation RuleDataOperationIfNoRulesAreMatched { get; set; } = RuleDataOperation.Query | RuleDataOperation.Delete | RuleDataOperation.Edit;

        public AbpRuleOptions() 
        {
            RuleExtraFieldManager=new RuleExtraFieldManager();

            RuleExtraFieldManager.AddDataExtraProperty<IOrganizationCode>(nameof(IOrganizationCode.OrganizationCode), ONEAbpClaimTypes.OrganizationCode, RuleFieldsValueNameConst.LoginUserOrganization);
            RuleExtraFieldManager.AddUserIdProperty(RuleFieldsValueNameConst.LoginUserId, user => user.Id);

            //UserRuleExtraFieldNameManager =new UserRuleExtraFieldNameManager();
            //DataRulePredefineFieldValueManager=new DataRulePredefineFieldValueManager();
            DataTargetOption= new DataTargetOption();
        }
    }
}
