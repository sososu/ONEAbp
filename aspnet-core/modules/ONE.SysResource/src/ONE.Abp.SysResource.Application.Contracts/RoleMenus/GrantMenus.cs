using System;
using System.Collections.Generic;

namespace ONE.Abp.SysResource.RoleMenus
{
    public class GrantMenus
    {
        public IList<string> CheckedKeys { get; set; }

        public IList<MenuTreeLabel> Menus { get; set; }
    }

    public class GrantApps
    {
        public string Label { get; set; }   
        public Guid Id { get; set; }    

        public bool IsCehcked { get; set; }
    }
}
