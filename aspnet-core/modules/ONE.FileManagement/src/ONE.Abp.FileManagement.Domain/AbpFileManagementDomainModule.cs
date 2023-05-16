using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace ONE.Abp.FileManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpFileManagementDomainSharedModule)
)]
public class AbpFileManagementDomainModule : AbpModule
{

}
