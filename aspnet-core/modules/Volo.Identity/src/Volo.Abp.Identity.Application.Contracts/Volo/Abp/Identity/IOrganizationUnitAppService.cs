using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<OrganizationUnitDto> GetAsync(Guid id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task CreateAsync(OrganizationUnitCreateInput input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task UpdateAsync(OrganizationUnitUpdateInput input);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteAsync(Guid id);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public Task<ListResultDto<OrganizationUnitDto>> GetListAsync(string name=null);


        /// <summary>
        /// 查询部门列表（排除节点）
        /// </summary>
        /// <returns></returns>
        public Task<ListResultDto<OrganizationUnitDto>> GetExcludeChild(Guid id);

        public Task<ListResultDto<TreeLabel>> GetOrgTreeAsync();


        public Task<ListResultDto<OrganizationUnitDto>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId, bool includeDetails = false);

        public Task<ListResultDto<OrganizationUnitDto>> GetChildrenAsync(
       Guid? parentId,
       bool includeDetails = false);


        public Task<ListResultDto<IdentityUserDto>> GetMembersAsync(
     string displayName,
     string sorting = null,
     int pageSize = int.MaxValue,
     int pageIndex = 0,
     string filter = null,
     bool includeDetails = false
 );

        public Task<int> GetMembersCountAsync(
           string displayName,
            string filter = null
        );
    }
}
