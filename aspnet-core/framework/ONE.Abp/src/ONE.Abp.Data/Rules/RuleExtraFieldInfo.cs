using ONE.Abp.Shared.Rules;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.Users;

namespace ONE.Abp.Data.Rules
{
    public class RuleExtraFieldInfo
    {
        /// <summary>
        /// 扩展属性名称
        /// </summary>
        public string ExtraPropertyName { get; set; }

        /// <summary>
        /// 声明名称 默认格式ExtraPropertyName小写
        /// </summary>
        public string ClaimName { get; set; }

        /// <summary>
        /// 预定义名称 默认格式{ExtraPropertyName}
        /// </summary>
        public string PredefineName { get; set; }


        /// <summary>
        /// 影子属性类型
        /// </summary>
        public Type ShadowPropertyType { get; set; }


        /// <summary>
        /// 获取预定义值委托
        /// </summary>
        public Func<ICurrentUser, object> GetPredefineValueFunc { get; set; }

    }


    public class RuleExtraFieldManager
    {
        private readonly List<RuleExtraFieldInfo> ruleExtraFields;

        public RuleExtraFieldManager()
        {
            ruleExtraFields= new List<RuleExtraFieldInfo>();
        }
        

        public IReadOnlyCollection<RuleExtraFieldInfo> GetRuleExtraFields()
        {
            return ruleExtraFields.ToImmutableList<RuleExtraFieldInfo>();
        }



        public IReadOnlyCollection<string> GetPredefineNames()
        {
           return ruleExtraFields.Where(r=>r.PredefineName!=null).Select(s => s.PredefineName).ToImmutableList();
        }


        public RuleExtraFieldInfo GetRuleExtraFieldInfoByPredefine(string defineName)
        {
            return ruleExtraFields.Where(r => r.PredefineName == defineName).FirstOrDefault();
        }


        public IReadOnlyCollection<RuleExtraFieldInfo> GetRuleExtraFieldForClaims()
        {
            return ruleExtraFields.Where(r => r.ClaimName!=null && r.ExtraPropertyName!=null ).ToImmutableList();
        }

        public IReadOnlyCollection<RuleExtraFieldInfo> GetRuleExtraFieldForShadow()
        {
            return ruleExtraFields.Where(r => r.ShadowPropertyType != null).ToImmutableList();
        }



        public void AddUserExtraProperty(string extraPropertyName)
        {
            AddUserExtraProperty(extraPropertyName, extraPropertyName.ToLower());
        }

        public void AddUserExtraProperty(string extraPropertyName, string claimName)
        {
            var field = ruleExtraFields.FirstOrDefault(f => f.ExtraPropertyName == extraPropertyName);
            if (field == null)
                ruleExtraFields.Add(new RuleExtraFieldInfo { ExtraPropertyName = extraPropertyName, ClaimName = claimName });
            else
            {
                field.ExtraPropertyName = extraPropertyName;
                field.ClaimName = claimName;
            }
        }

        public void AddDataExtraProperty<TInterface>(string extraPropertyName, Func<ICurrentUser, object> getPredefineValueFunc = null) where TInterface : IShadowProperty
        {
            AddDataExtraProperty<TInterface>(extraPropertyName, extraPropertyName.ToLower(), $"{{{extraPropertyName}}}", getPredefineValueFunc);
        }

        public void AddDataExtraProperty<TInterface>(string extraPropertyName, string predefineName, Func<ICurrentUser, object> getPredefineValueFunc = null) where TInterface : IShadowProperty
        {
            AddDataExtraProperty<TInterface>(extraPropertyName, extraPropertyName.ToLower(),predefineName, getPredefineValueFunc);
        }

        public void AddDataExtraProperty<TInterface>(string extraPropertyName, string claimName,string predefineName, Func<ICurrentUser, object> getPredefineValueFunc = null) where TInterface:IShadowProperty
        {
            AddDataExtraProperty(typeof(TInterface),extraPropertyName,claimName,predefineName, getPredefineValueFunc);
        }


        public void AddDataExtraProperty(Type type,string extraPropertyName, string claimName, string predefineName, Func<ICurrentUser, object> getPredefineValueFunc = null)
        {
            if (getPredefineValueFunc == null)
            {
                getPredefineValueFunc = user => { return user.FindClaimValue(claimName); };
            }

            var field = ruleExtraFields.FirstOrDefault(f => f.ExtraPropertyName == extraPropertyName);
            if (field == null)
                ruleExtraFields.Add(new RuleExtraFieldInfo { ExtraPropertyName = extraPropertyName, ClaimName = claimName, PredefineName = predefineName, ShadowPropertyType = type, GetPredefineValueFunc = getPredefineValueFunc });
            else
            {
                field.ExtraPropertyName = extraPropertyName;
                field.ClaimName = claimName;
                field.PredefineName = predefineName;
                field.ShadowPropertyType = type;
                field.GetPredefineValueFunc = getPredefineValueFunc;
            }
        }

        internal void AddUserIdProperty(string predefineName, Func<ICurrentUser, object> getPredefineValueFunc)
        {
            ruleExtraFields.Add(new RuleExtraFieldInfo { PredefineName = predefineName,GetPredefineValueFunc = getPredefineValueFunc });
        }
    }
}
