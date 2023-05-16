using System;
using System.Collections.Generic;

namespace ONE.Abp.SysResource.RoleMenus
{
    public class RoleMenuCreateInput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public Guid AppId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 菜单编码集合
        /// </summary>
        public ICollection<string> MenuCodes { get; set; }
    }
}
