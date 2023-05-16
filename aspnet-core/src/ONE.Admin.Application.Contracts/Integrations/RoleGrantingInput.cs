using System;
using System.Collections.Generic;

namespace ONE.Admin.Integrations
{
    public class RoleGrantingInput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public Guid AppId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单编码集合
        /// </summary>
        public ICollection<string> MenuCodes { get; set; }
    }
}
