using Microsoft.Extensions.DependencyInjection;

namespace MyCompanyName.MyProjectName
{
    public static class OpenIddictServerBuilderExtension
    {
        //定义一个扩展方法
        public static OpenIddictServerBuilder AllowExchangeTokenFlow(this OpenIddictServerBuilder builder)
        {
            return builder.AllowCustomFlow(ExchangeTokenExtensionGrantConsts.GrantType);
        }
    }
}
