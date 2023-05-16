using ONE.Abp.Data.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission.Rules
{
    public class RuleSeed:IDataSeedContributor
    {
        protected IGuidGenerator GuidGenerator;
        public RuleSeed(IGuidGenerator guidGenerator) 
        {
            GuidGenerator=guidGenerator;
        }

        public UserRule GetEveryOne()
        {



            var rule= new UserRule(GuidGenerator.Create(), "所有人");
            //rule.Condition =new List<ConditionGroupUnit>
            //    {
            //        new ConditionGroupUnit
            //        {
            //           Name="认证通过的",
            //           ConditionUnits=new System.Collections.Generic.List<ConditionUnit>
            //           {
            //               new ConditionUnit
            //               {
            //                   Condition=new Condition
            //                   {
            //                       Compare=Abp.Data.QueryCompare.Equal,
            //                       FieldName=nameof(ICurrentUser.IsAuthenticated),
            //                       FieldValue="true",
            //                   }
            //               }
            //           },
            //        }
            //    },
            //rule.Condition = "";
            return rule;
        }

        public Task SeedAsync(DataSeedContext context)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
