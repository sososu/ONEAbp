using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ONE.Abp.Data;
using ONE.Abp.Shared;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace ONE.Abp.DataPermission.Tests
{
    public class DataPermissionTestBase : AbpIntegratedTest<DataPermissionTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var claims = new List<Claim>() {
                                    new Claim(AbpClaimTypes.UserId,"2e701e62-0953-4dd3-910b-dc6cc93ccb0d"),
                                    new Claim(AbpClaimTypes.UserName,"admin"),
                                    new Claim(AbpClaimTypes.Email,"admin@abp.io"),
                                    new Claim(AbpClaimTypes.Role,"admin"),
                                    new Claim(AbpClaimTypes.Role,"superadmin"),
                                    new Claim(ONEAbpClaimTypes.OrganizationCode,"V")
                            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var principalAccessor = Substitute.For<ICurrentPrincipalAccessor>();
            principalAccessor.Principal.Returns(ci => claimsPrincipal);
            Thread.CurrentPrincipal = claimsPrincipal;
        }



        protected virtual Task WithUnitOfWorkAsync(Func<Task> func)
        {
            return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }

        protected virtual async Task WithUnitOfWorkAsync(AbpUnitOfWorkOptions options, Func<Task> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin(options))
                {
                    await action();

                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
        {
            return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }

        protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(AbpUnitOfWorkOptions options, Func<Task<TResult>> func)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin(options))
                {
                    var result = await func();
                    await uow.CompleteAsync();
                    return result;
                }
            }
        }
    }
}
