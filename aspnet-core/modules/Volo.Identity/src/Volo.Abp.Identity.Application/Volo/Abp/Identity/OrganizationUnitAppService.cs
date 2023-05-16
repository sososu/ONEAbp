using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// 组织单位服务
    /// </summary>
    [Authorize]
    public class OrganizationUnitAppService : IdentityAppServiceBase, IOrganizationUnitAppService
    {
        protected OrganizationUnitManager OrganizationUnitManager { get; }
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IOrganizationUnitRepository organizationUnitRepository)
        {
            OrganizationUnitManager = organizationUnitManager;
            OrganizationUnitRepository = organizationUnitRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.OrganizationUnit.Create)]
        public async Task CreateAsync(OrganizationUnitCreateInput input)
        {
            var organizationUnit = new OrganizationUnit(GuidGenerator.Create(), input.DisplayName, input.ParentId, CurrentTenant.Id);
            input.MapExtraPropertiesTo(organizationUnit);
            await OrganizationUnitManager.CreateAsync(organizationUnit);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.OrganizationUnit.Update)]
        public async Task UpdateAsync(OrganizationUnitUpdateInput input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName;
            organizationUnit.SetParentId(input.ParentId);
            await OrganizationUnitManager.UpdateAsync(organizationUnit);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.OrganizationUnit.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await OrganizationUnitManager.DeleteAsync(id);
        }

        public async Task<OrganizationUnitDto> GetAsync(Guid id)
        {
            var org = await OrganizationUnitRepository.FindAsync(id);
            if (org == null)
                return null;

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(org);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<OrganizationUnitDto>> GetListAsync(string name = null)
        {
            var list = await OrganizationUnitRepository.GetListAsync(name);
            return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
        }

        /// <summary>
        /// 查询部门列表（排除节点）
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<OrganizationUnitDto>> GetExcludeChild(Guid id)
        {
            var list = await OrganizationUnitRepository.GetListAsync();
            var os = await OrganizationUnitManager.FindChildrenAsync(id);
            var osIds = os.Select(o => o.Id);

            list.RemoveAll(d => d.Id == id || osIds.Contains(d.Id));
            return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
        }




        public async Task<ListResultDto<OrganizationUnitDto>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId, bool includeDetails = false)
        {
            var list = await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(code, parentId, includeDetails);
            return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
        }

        public async Task<ListResultDto<OrganizationUnitDto>> GetChildrenAsync(
       Guid? parentId,
       bool includeDetails = false)
        {
            var list = await OrganizationUnitRepository.GetChildrenAsync(parentId, includeDetails);
            return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
        }


        public async Task<ListResultDto<IdentityUserDto>> GetMembersAsync(
     string displayName,
     string sorting = null,
     int pageSize = int.MaxValue,
     int pageIndex = 0,
     string filter = null,
     bool includeDetails = false
 )
        {
            var org = await OrganizationUnitRepository.GetAsync(displayName, includeDetails);
            var list = await OrganizationUnitRepository.GetMembersAsync(org, sorting, pageSize, (pageIndex - 1) * pageSize, filter, includeDetails);
            return new ListResultDto<IdentityUserDto>(ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list));
        }

        public async Task<int> GetMembersCountAsync(
           string displayName,
            string filter = null
        )
        {
            var org = await OrganizationUnitRepository.GetAsync(displayName);
            return await OrganizationUnitRepository.GetMembersCountAsync(org, filter);
        }


        public async Task<ListResultDto<TreeLabel>> GetOrgTreeAsync()
        {
            var orgs = ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(await OrganizationUnitRepository.GetListAsync());
            return new ListResultDto<TreeLabel>(BuildTreeLabel(orgs));
        }

        protected virtual List<TreeLabel> BuildTreeLabel(List<OrganizationUnitDto> units)
        {
            List<TreeLabel> list = new List<TreeLabel>();

            var rootUnit = units.Where(p => !p.ParentId.HasValue).ToList();
            foreach (var unit in rootUnit)
            {
                var tree = new TreeLabel { Id = unit.Id, Label = unit.DisplayName };

                RecursionLabelFn(units, tree);

                list.Add(tree);
            }

            return list;
        }

        protected virtual void RecursionLabelFn(List<OrganizationUnitDto> units, TreeLabel tree)
        {
            var ms = units.Where(m => m.ParentId == tree.Id).Select(m => new TreeLabel { Id = m.Id, Label = m.DisplayName }).ToList();

            foreach (var mm in ms)
            {
                RecursionLabelFn(units, mm);
            }

            tree.Children = ms;
        }

    }
}

