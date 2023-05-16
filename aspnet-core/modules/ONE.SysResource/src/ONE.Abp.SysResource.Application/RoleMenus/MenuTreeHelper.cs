using Volo.Abp.ObjectMapping;
using ONE.Abp.SysResource.SysMenus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ONE.Abp.SysResource.RoleMenus
{
    internal class MenuTreeHelper
    {
        internal static List<MenuTreeLabel> BuildMenuTreeLabel(List<SysMenuDto> menus)
        {
            List<MenuTreeLabel> list = new List<MenuTreeLabel>();

            var rootMenus = menus.Where(p => p.ParentCode.IsNullOrWhiteSpace()).OrderBy(o => o.Order).ToList();
            foreach (var menu in rootMenus)
            {
                var tree = new MenuTreeLabel { Code = menu.Code, Label = menu.Name };

                RecursionLabelFn(menus, tree);

                list.Add(tree);
            }

            return list;
        }

        internal static void RecursionLabelFn(List<SysMenuDto> menus, MenuTreeLabel menu)
        {
            var ms = menus.Where(m => m.ParentCode == menu.Code).OrderBy(o => o.Order).Select(mm => new MenuTreeLabel { Code = mm.Code, Label = mm.Name }).ToList();

            foreach (var mm in ms)
            {
                RecursionLabelFn(menus, mm);
            }

            menu.Children = ms;
        }


        internal static List<MenuTree> BuildMenuTree(List<SysMenuDto> menus,IObjectMapper objectMapper)
        {
            List<MenuTree> list = new List<MenuTree>();

            var rootMenus = menus.Where(p => p.ParentCode.IsNullOrWhiteSpace()).OrderBy(o => o.Order).ToList();
            foreach (var menu in rootMenus)
            {
                var tree = objectMapper.Map<SysMenuDto, MenuTree>(menu);

                RecursionFn(menus, tree,objectMapper);

                list.Add(tree);
            }

            return list;
        }

        internal static void RecursionFn(List<SysMenuDto> menus, MenuTree menu,IObjectMapper objectMapper)
        {
            var ms = menus.Where(m => m.ParentCode == menu.Code).OrderBy(o => o.Order).Select(m => objectMapper.Map<SysMenuDto, MenuTree>(m)).ToList();

            foreach (var mm in ms)
            {
                RecursionFn(menus, objectMapper.Map<SysMenuDto, MenuTree>(mm),objectMapper);
            }

            menu.Children = ms;
        }
    }
}
