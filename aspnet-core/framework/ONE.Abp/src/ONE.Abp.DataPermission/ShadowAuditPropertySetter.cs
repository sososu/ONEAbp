using Microsoft.Extensions.Options;
using ONE.Abp.Data.Rules;
using System.Linq;
using System.Reflection;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission
{
    public class ShadowAuditPropertySetter : AuditPropertySetter
    {
        protected AbpRuleOptions AbpRuleOptions { get; }

        public ShadowAuditPropertySetter(ICurrentUser currentUser, ICurrentTenant currentTenant, IClock clock, IOptions<AbpRuleOptions> options) : base(currentUser, currentTenant, clock)
        {
            AbpRuleOptions = options.Value;
        }


        public override void SetCreationProperties(object targetObject)
        {
            base.SetCreationProperties(targetObject);


            if (!CurrentUser.Id.HasValue)
            {
                return;
            }

            if (targetObject is IMultiTenant multiTenantEntity)
            {
                if (multiTenantEntity.TenantId != CurrentUser.TenantId)
                {
                    return;
                }
            }

            var extraFields = AbpRuleOptions.RuleExtraFieldManager.GetRuleExtraFieldForShadow();
            foreach (var item in extraFields)
            {
                if (item.ShadowPropertyType.IsAssignableFrom(targetObject.GetType()))
                {
                    var p = item.ShadowPropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();
                    var property = targetObject.GetType().GetProperty(p.Name);
                    object value = property.GetValue(targetObject);

                    if (value != null && value != default)
                    {
                        return;
                    }

                    property.SetValue(targetObject,item.GetPredefineValueFunc(CurrentUser));
                }
            }

        }
    }
}
