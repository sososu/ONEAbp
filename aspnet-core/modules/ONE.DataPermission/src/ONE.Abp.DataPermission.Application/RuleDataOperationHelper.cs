using ONE.Abp.Shared.Rules;
using System;
using System.Collections.Generic;

namespace ONE.Abp.DataPermission
{
    public class RuleDataOperationHelper
    {
        public static IList<RuleDataOperation> FromRuleDataOperation(RuleDataOperation option)
        {
            var cates = new List<RuleDataOperation>();
            foreach (RuleDataOperation item in Enum.GetValues(typeof(RuleDataOperation)))
            {
                if (option.HasFlag(item))
                {
                    cates.Add(item);
                }
            }
            return cates;
        }
    }
}
