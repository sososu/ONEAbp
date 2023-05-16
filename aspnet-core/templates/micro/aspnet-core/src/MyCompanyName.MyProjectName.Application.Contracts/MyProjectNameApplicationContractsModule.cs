using ONE.Abp.Pagination.Contracts;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(MyProjectNameDomainSharedModule),
    typeof(AbpObjectExtendingModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpPaginationContractsModule)
    )]
public class MyProjectNameApplicationContractsModule : AbpModule
{
}
