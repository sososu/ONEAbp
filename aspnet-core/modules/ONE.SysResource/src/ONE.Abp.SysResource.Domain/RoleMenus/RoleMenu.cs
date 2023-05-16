using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.SysResource.RoleMenus
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class RoleMenu : AggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        ///角色
        /// </summary>
        public virtual string Role { get; protected set; }

        ///// <summary>
        ///// Gets or sets the primary key of the role that is linked to the Role.
        ///// </summary>
        //public virtual Guid MenuId { get; protected set; }


        public virtual Guid AppId { get; protected set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public virtual string MenuCode { get; protected set; }

        public Guid? TenantId { get; protected set; }

        protected RoleMenu()
        {

        }

        public RoleMenu(Guid id, string role, Guid appId, string menuCode)
        {
            Id = id;
            AppId = appId;
            Role = role;
            MenuCode = menuCode;
        }

        public void ChangeRole(string role)
        {
            Role = role;
        }
    }
}
