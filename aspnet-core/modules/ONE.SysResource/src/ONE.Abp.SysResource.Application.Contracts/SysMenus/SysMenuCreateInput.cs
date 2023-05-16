using JetBrains.Annotations;
using ONE.Abp.SysResource.Menus;
using System;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.SysResource.SysMenus
{
    public class SysMenuCreateInput
    {
        /// <summary>
        /// 菜单名
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }


        /// <summary>
        /// 菜单编码
        /// </summary>
        [Required]
        [MaxLength(256)]

        public string Code { get; set; }


        /// <summary>
        /// 显示顺序 从小到大
        /// </summary>
        [Required]
        public int Order { get; set; }

        /// <summary>
        /// 父级菜单编码
        /// </summary>
        [MaxLength(256)]
        public string ParentCode { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [MaxLength(256)]
        public string Path { get; set; }


        /// <summary>
        /// 组件路径
        /// </summary>
        [MaxLength(256)]
        public string Component { get; set; }


        /// <summary>
        /// 路由参数
        /// </summary>
        [MaxLength(256)]
        public string Query { get; set; }


        /// <summary>
        ///  是否为外链
        /// </summary>
        [Required]
        public bool IsFrame { get; set; }


        /// <summary>
        ///  是否缓存
        /// </summary>
        [Required]
        public bool IsCache { get; set; }

        /// <summary>
        /// 是否启用 默认True
        /// </summary>
        [Required]
        public bool IsEnable { get; set; }


        /// <summary>
        /// 显示状态 默认True
        /// </summary>
        [Required]
        public bool Visible { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [CanBeNull]
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public MenuType MenuType { get; set; }


        /// <summary>
        /// 系统应用Id
        /// </summary>
        [Required]
        public Guid SysAppId { get; set; }


        /// <summary>
        /// 权限字符串
        /// </summary>
        public string Perms { get; set; }
    }

    public class SysMenuUpdateInput : SysMenuCreateInput
    {
       
    }
}
