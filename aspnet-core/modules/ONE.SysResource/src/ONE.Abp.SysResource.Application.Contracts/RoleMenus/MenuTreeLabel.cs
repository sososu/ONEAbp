using System;
using System.Collections.Generic;
using System.Text;

namespace ONE.Abp.SysResource.RoleMenus
{
    public class MenuTreeLabel
    {
        public string Code { get; set; }

        public string Label { get; set; }

        public IList<MenuTreeLabel> Children { get; set; }
    }
}
