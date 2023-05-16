using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission
{
    public class UserExtraFieldResolver : IUserExtraFieldResolver, ITransientDependency
    {
        readonly MethodInfo _createValueTupleMethod =
 typeof(CurrentUserExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m => m.Name == nameof(CurrentUserExtensions.FindClaimValue) && !m.IsGenericMethod);

        public virtual Expression GetExpression(string name, Expression user)
        {
            //todo:是否支持集合类型？

            var param = Expression.Constant(name);
            return Expression.Call(null, _createValueTupleMethod, user, param);
        }
    }

    public interface IUserExtraFieldResolver
    {
        Expression GetExpression(string name, Expression user);
    }
}
