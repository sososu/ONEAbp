using Microsoft.Extensions.DependencyInjection;

namespace ONE.Admin
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
