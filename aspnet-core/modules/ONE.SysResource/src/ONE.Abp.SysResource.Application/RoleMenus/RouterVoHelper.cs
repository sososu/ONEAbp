using ONE.Abp.Shared.Utils;
using ONE.Abp.SysResource.Menus;
using ONE.Abp.SysResource.SysMenus;
using System;

namespace ONE.Abp.SysResource.RoleMenus
{
    public class RouterVoHelper
    {
        /// <summary>
        /// 获取路由名称
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static string GetRouteName(SysMenuDto menu)
        {
            string routerName = menu.Path.UpperFirstChar();
            // 非外链并且是一级目录（类型为目录）
            if (IsMenuFrame(menu))
            {
                routerName = string.Empty;
            }
            return routerName;
        }


        /// <summary>
        /// 获取路由地址
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static string GetRouterPath(SysMenuDto menu)
        {
            string routerPath = menu.Path;
            // 内链打开外网方式
            if (menu.ParentCode.IsNotNullOrWhiteSpace() && IsInnerLink(menu))
            {
                routerPath = InnerLinkReplaceEach(routerPath);
            }
            // 非外链并且是一级目录（类型为目录）
            if (menu.ParentCode.IsNullOrWhiteSpace() && menu.MenuType == MenuType.M
                    && !menu.IsFrame)
            {
                routerPath = "/" + menu.Path;
            }
            // 非外链并且是一级目录（类型为菜单）
            else if (IsMenuFrame(menu))
            {
                routerPath = "/";
            }
            return routerPath;
        }



        /// <summary>
        ///  获取组件信息
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static string GetComponent(SysMenuDto menu)
        {
            string component = "Layout";
            if (menu.Component.IsNotNullOrWhiteSpace() && !IsMenuFrame(menu))
            {
                component = menu.Component;
            }
            else if (menu.Component.IsNullOrWhiteSpace() && menu.ParentCode.IsNotNullOrWhiteSpace() && IsInnerLink(menu))
            {
                component = "InnerLink";
            }
            else if (menu.Component.IsNullOrWhiteSpace() && IsParentView(menu))
            {
                component = "ParentView";
            }
            return component;
        }


        /// <summary>
        /// 是否为菜单内部跳转
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static bool IsMenuFrame(SysMenuDto menu)
        {
            return menu.ParentCode.IsNullOrWhiteSpace() && MenuType.C == menu.MenuType
                    && !menu.IsFrame;
        }

        /// <summary>
        /// 是否为内链组件
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static bool IsInnerLink(SysMenuDto menu)
        {
            return !menu.IsFrame && menu.Path.IsUrl();
        }

        /// <summary>
        /// 是否为parent_view组件
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static bool IsParentView(SysMenuDto menu)
        {
            return menu.ParentCode.IsNotNullOrWhiteSpace() && MenuType.M == menu.MenuType;
        }


        /// <summary>
        ///  内链域名特殊字符替换
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string InnerLinkReplaceEach(string path)
        {
            return path.Replace(SysResourceConst.HTTP, "").Replace(SysResourceConst.HTTPS, "").Replace(SysResourceConst.WWW, "")
                .Replace(".", "/");
        }
    }
}
