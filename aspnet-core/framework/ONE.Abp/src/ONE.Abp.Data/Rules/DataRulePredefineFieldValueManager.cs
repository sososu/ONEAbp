//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Volo.Abp.Users;

//namespace ONE.Abp.Data.Rules
//{
//    /// <summary>
//    /// 数据规则字段预定义值管理，要提到抽象底层，其他微服务可以调用
//    /// </summary>
//    public class DataRulePredefineFieldValueManager
//    {
//        Dictionary<string,Func<ICurrentUser,object>>DefaultFieldsValueFunc=new Dictionary<string,Func<ICurrentUser,object>>();

//        public DataRulePredefineFieldValueManager() 
//        {
//            DefaultFieldsValueFunc.Add(RuleFieldsValueNameConst.LoginUserId, user => user.Id);
//            DefaultFieldsValueFunc.Add(RuleFieldsValueNameConst.LoginUserOrganization, user => user.FindClaimValue(ONEAbpClaimTypes.OrganizationCode));
//        }

//        public bool ContainsKey(string key)
//        {
//            return DefaultFieldsValueFunc.ContainsKey(key);
//        }

//        public string[] GetKeys()
//        {
//            return DefaultFieldsValueFunc.Keys.ToArray();
//        }

//        public Func<ICurrentUser, object> GetValueFunc(string key)
//        {
//            return DefaultFieldsValueFunc.ContainsKey(key) ? DefaultFieldsValueFunc[key] : null;
//        }

//        public Func<ICurrentUser, object> GetValueFuncOrDefault(string key)
//        {
//            return DefaultFieldsValueFunc.ContainsKey(key) ? DefaultFieldsValueFunc[key] : user => key;
//        }

//        public void Add(string key, Func<ICurrentUser, object> func)
//        {
//            if (DefaultFieldsValueFunc.ContainsKey(key))
//                return;

//            DefaultFieldsValueFunc.Add(key, func);
//        }

//        public void AddOrReplace(string key, Func<ICurrentUser, object> func)
//        {
//            if (DefaultFieldsValueFunc.ContainsKey(key))
//                DefaultFieldsValueFunc.Remove(key);

//            DefaultFieldsValueFunc.Add(key, func);
//        }
//    }

//}
