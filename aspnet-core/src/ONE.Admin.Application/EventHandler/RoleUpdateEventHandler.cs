using ONE.Abp.SysResource.RoleMenus;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace ONE.Admin.EventHandler;

public class RoleUpdateEventHandler :
    IDistributedEventHandler<IdentityRoleNameChangedEto>,
    ITransientDependency
{
    private readonly IRepository<RoleMenu> _roleMenuRepository;

    public RoleUpdateEventHandler(IRepository<RoleMenu> roleMenuRepository)
    {
        _roleMenuRepository = roleMenuRepository;
    }

    public async Task HandleEventAsync(IdentityRoleNameChangedEto eventData)
    {
        var roleMenus = await _roleMenuRepository.GetListAsync(r => r.Role == eventData.OldName);
        foreach (var roleMenu in roleMenus)
        {
            roleMenu.ChangeRole(eventData.Name);
            await _roleMenuRepository.UpdateAsync(roleMenu);
        }
    }
}
