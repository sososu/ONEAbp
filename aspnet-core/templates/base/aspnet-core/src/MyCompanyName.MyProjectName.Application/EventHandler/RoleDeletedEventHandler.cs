using ONE.Abp.SysResource.RoleMenus;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace MyCompanyName.MyProjectName.EventHandler;

public class RoleDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<IdentityRoleEto>>,
    ITransientDependency
{
    private readonly IRepository<RoleMenu> _roleMenuRepository;

    public RoleDeletedEventHandler(IRepository<RoleMenu> roleMenuRepository)
    {
        _roleMenuRepository= roleMenuRepository;
    }

    public async Task HandleEventAsync(EntityDeletedEto<IdentityRoleEto> eventData)
    {
        await _roleMenuRepository.DeleteAsync(r => r.Role == eventData.Entity.Name);
    }
}
