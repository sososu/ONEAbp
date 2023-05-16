using ONE.Abp.SysResource.SysMenus;
using System.Collections.Generic;

namespace ONE.Abp.SysResource.RoleMenus
{
    public class MenuTree:SysMenuDto
    {
        public IList<MenuTree> Children { get; set; }
    }

   
}
