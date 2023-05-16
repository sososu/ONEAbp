//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ONE.Abp.Data.Rules
//{
//    /// <summary>
//    /// 用户规则扩展字段名称管理
//    /// </summary>
//    public class UserRuleExtraFieldNameManager
//    {
//        Dictionary<string, Type> UserRuleExtraFieldKv = new Dictionary<string, Type>();


//        public UserRuleExtraFieldNameManager()
//        {
//            Add(ONEAbpClaimTypes.OrganizationCode);
//        }


//        public bool ContainsKey(string key)
//        {
//            return UserRuleExtraFieldKv.ContainsKey(key);
//        }

//        public string[] GetKeys()
//        {
//            return UserRuleExtraFieldKv.Keys.ToArray();
//        }


//        public ValueTuple<string, Type>[] GetAll()
//        {
//            return UserRuleExtraFieldKv.Select(kv => (kv.Key, kv.Value)).ToArray();
//        }

//        public void Add(string key)
//        {
//            if (UserRuleExtraFieldKv.ContainsKey(key))
//                return;

//            UserRuleExtraFieldKv.Add(key, typeof(string));
//        }

//        /// <summary>
//        /// 目前只支持字符串类型比较，暂时没有支持对其他类型的比较
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="type"></param>
//        internal void Add(string key, Type type)
//        {
//            if (UserRuleExtraFieldKv.ContainsKey(key))
//                return;
//            UserRuleExtraFieldKv.Add(key, type);
//        }
//    }
//}
