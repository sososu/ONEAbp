using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
namespace Volo.Abp.Identity
{
    /// <summary>
    /// 组织单位服务
    /// </summary>
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area(IdentityRemoteServiceConsts.ModuleName)]
    [ControllerName("OrganizationUnit")]
    [Route("api/identity/organizations")]
    public class OrganizationUnitController : AbpControllerBase, IOrganizationUnitAppService
    {
        protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

        public OrganizationUnitController(IOrganizationUnitAppService organizationUnitAppService)
        {
            OrganizationUnitAppService = organizationUnitAppService;
        }

        /// <summary>
        /// 创建组织单位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task CreateAsync(OrganizationUnitCreateInput input)
        {
            return OrganizationUnitAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新组织单位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public Task UpdateAsync(OrganizationUnitUpdateInput input)
        {
            return OrganizationUnitAppService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除组织单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return OrganizationUnitAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取组织单位列表
        /// </summary>
        /// <param name="name">部门名称</param>
        /// <returns></returns>
        [HttpGet("list")]
        public Task<ListResultDto<OrganizationUnitDto>> GetListAsync(string name=null)
        {
            return OrganizationUnitAppService.GetListAsync(name);
        }

        /// <summary>
        /// 查询组织单位列表（排除节点）
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns></returns>
        [HttpGet("list/exclude/{id}")]
        public Task<ListResultDto<OrganizationUnitDto>> GetExcludeChild(Guid id)
        {
            return OrganizationUnitAppService.GetExcludeChild(id);
        }


        /// <summary>
        /// 获取组织单位树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tree")]
        public Task<ListResultDto<TreeLabel>> GetOrgTreeAsync()
        {
            return OrganizationUnitAppService.GetOrgTreeAsync();
        }

        /// <summary>
        /// 获取组织
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{id}")]
        public Task<OrganizationUnitDto> GetAsync(Guid id)
        {
            return OrganizationUnitAppService.GetAsync(id);
        }

        [HttpGet("all-children")]
        public Task<ListResultDto<OrganizationUnitDto>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId, bool includeDetails = false)
        {
            return OrganizationUnitAppService.GetAllChildrenWithParentCodeAsync(code, parentId, includeDetails);
        }

        [HttpGet("children")]
        public Task<ListResultDto<OrganizationUnitDto>> GetChildrenAsync(Guid? parentId, bool includeDetails = false)
        {
            return OrganizationUnitAppService.GetChildrenAsync(parentId, includeDetails);
        }

        [HttpGet("members")]
        public Task<ListResultDto<IdentityUserDto>> GetMembersAsync(string displayName, string sorting = null, int pageSize = int.MaxValue, int pageIndex = 0, string filter = null, bool includeDetails = false)
        {
            return OrganizationUnitAppService.GetMembersAsync(displayName, sorting,pageSize,pageIndex,filter,includeDetails);
        }
        

        [HttpGet("members-count")]
        public Task<int> GetMembersCountAsync(string displayName, string filter = null)
        {
            return OrganizationUnitAppService.GetMembersCountAsync(displayName, filter);
        }
    }
}
