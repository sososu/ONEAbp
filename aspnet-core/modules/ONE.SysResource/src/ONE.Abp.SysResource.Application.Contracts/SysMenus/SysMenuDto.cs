using JetBrains.Annotations;
using ONE.Abp.SysResource.Menus;
using System;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.SysResource.SysMenus
{
    public class SysMenuDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 显示顺序 从小到大
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父级菜单编码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }


        /// <summary>
        /// 路由参数
        /// </summary>
        public string Query { get; set; }


        /// <summary>
        ///  是否为外链
        /// </summary>
        public bool IsFrame { get; set; }


        /// <summary>
        ///  是否缓存
        /// </summary>
        public bool IsCache { get; set; }

        /// <summary>
        /// 是否启用 默认True
        /// </summary>
        public bool IsEnable { get; set; }


        /// <summary>
        /// 显示状态 默认True
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [CanBeNull]
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MenuType MenuType { get; set; }


        /// <summary>
        /// 系统应用Id
        /// </summary>
        public Guid SysAppId { get; set; }


        /// <summary>
        /// 权限字符串
        /// </summary>
        public string Perms { get; set; }

    }
}
