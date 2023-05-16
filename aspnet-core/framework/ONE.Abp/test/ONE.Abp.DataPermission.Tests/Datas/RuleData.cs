using ONE.Abp.Data.Rules;
using ONE.Abp.Shared;
using ONE.Abp.Shared.Rules;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission.Tests.Datas
{
    public class RuleData
    {
        public static UserRuleResult UserRuleResult=new UserRuleResult
        {
            Id=Guid.NewGuid(),
            Conditions = new List<ConditionGroupUnit>
                {
                    new ConditionGroupUnit
                    {
                       Name="认证通过的",
                       ConditionUnits=new List<ConditionUnit>
                       {
                           new ConditionUnit
                           {
                               Condition=new Condition
                               {
                                   Compare=QueryCompare.Equal,
                                   FieldName=nameof(ICurrentUser.IsAuthenticated),
                                   FieldValue="true",
                               }
                           }
                       },
                    }
                },
            Name = "所有人",
        };


        public static DataRuleResult DataRuleResult1 = new DataRuleResult
        {
            Id = Guid.NewGuid(),
            Conditions = new List<ConditionGroupUnit>
                {
                    new ConditionGroupUnit
                    {
                       Name="创建人等于登录用户并且是深圳用户",
                       ConditionUnits=new List<ConditionUnit>
                       {
                           new ConditionUnit
                           {
                               Condition=new Condition
                               {
                                   Compare=QueryCompare.Equal,
                                   FieldName=nameof(Customer.CreatorId),
                                   FieldValue=RuleFieldsValueNameConst.LoginUserId,
                               }
                           },
                             new ConditionUnit
                           {
                               Condition=new Condition
                               {
                                   Compare=QueryCompare.Equal,
                                   FieldName="Address.City",
                                   FieldValue="深圳",
                               },
                               ConditionOperator=ConditionOperator.And

                           }
                       },

                    }
                },
            DataOperation = RuleDataOperation.Query | RuleDataOperation.Delete | RuleDataOperation.Edit,
            Name = "只能操作自己的数据",
            DataTargetName = nameof(Customer),
            HideDataTargetFields = "Address.Detial,Address.City",
        };


        public static DataRuleResult DataRuleResult2 = new DataRuleResult
        {
            Id = Guid.NewGuid(),
            Conditions = new List<ConditionGroupUnit>
                {
                    new ConditionGroupUnit
                    {
                       Name="部门编码等于登录部门编码",
                       ConditionUnits=new List<ConditionUnit>
                       {
                           new ConditionUnit
                           {
                               Condition=new Condition
                               {
                                   Compare=QueryCompare.Equal,
                                   FieldName=nameof(Customer.OrganizationCode),
                                   FieldValue=RuleFieldsValueNameConst.LoginUserOrganization,
                               }
                           }
                       },
                    }
                },
            DataOperation = RuleDataOperation.Query,
            Name = "只能查看自己部门的数据",
            DataTargetName = nameof(Customer),
            HideDataTargetFields =$"Name,Mobile"
        };

        public static List<DataRuleResult> DataRuleResults = new List<DataRuleResult>
        {
           DataRuleResult1,
           DataRuleResult2,
        };

        public static List<UserDataRuleResult> UserDataRuleResults = new List<UserDataRuleResult>
        {

            new UserDataRuleResult
            {
                Id = Guid.NewGuid(),
                UserRuleId = UserRuleResult.Id,
                DataRuleId = DataRuleResult1.Id,
                DataTargetName = nameof(Customer),
                Priority = 100,
                RuleType = RuleType.Common,
                IsEnable=true,
            },

             new UserDataRuleResult
            {
                Id = Guid.NewGuid(),
                UserRuleId = UserRuleResult.Id,
                DataRuleId = DataRuleResult2.Id,
                DataTargetName = nameof(Customer),
                Priority = 1,
                RuleType = RuleType.Common,
                IsEnable=true,
            }
        };
    }
}
