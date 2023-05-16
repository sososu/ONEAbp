using JetBrains.Annotations;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using ONE.Abp.SysResource.Menus;
using System;

namespace ONE.Abp.SysResource.SysMenus
{
    [ExcelImporter(IsLabelingError = true)]
    public class MenuImport
    {
        /// <summary>
        /// 菜单名
        /// </summary>
        [ImporterHeader(Name = "Name")]
        public string Name { get; set; }


        /// <summary>
        /// 菜单编码
        /// </summary>
        [ImporterHeader(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// 父级菜单编码
        /// </summary>
        [ImporterHeader(Name = "ParentCode")]
        public string ParentCode { get; set; }


        /// <summary>
        /// 显示顺序 从小到大
        /// </summary>
        [ImporterHeader(Name = "Order")]
        public int Order { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [CanBeNull]
        [ImporterHeader(Name = "Path")]
        public string Path { get; set; }


        /// <summary>
        /// 组件路径
        /// </summary>
        [ImporterHeader(Name = "Component")]
        public string Component { get; set; }


        /// <summary>
        /// 路由参数
        /// </summary>
        [CanBeNull]
        [ImporterHeader(Name = "Query")]
        public string Query { get; set; }


        /// <summary>
        ///  是否为外链
        /// </summary>
        [ImporterHeader(Name = "IsFrame")]
        public bool IsFrame { get; set; }


        /// <summary>
        ///  是否缓存
        /// </summary>
        [ImporterHeader(Name = "IsCache")]
        public bool IsCache { get; set; }

        /// <summary>
        /// 是否启用 默认True
        /// </summary>
        [ImporterHeader(Name = "IsEnable")]
        public bool IsEnable { get; set; }


        /// <summary>
        /// 显示状态 默认True
        /// </summary>
        [ImporterHeader(Name = "Visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [CanBeNull]
        [ImporterHeader(Name = "Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [ImporterHeader(Name = "MenuType")]
        public MenuType MenuType { get; set; }


        /// <summary>
        /// 系统应用Id
        /// </summary>
        [ImporterHeader(Name = "SysAppId")]
        public Guid SysAppId { get; set; }


        /// <summary>
        /// 权限字符串
        /// </summary>
        [CanBeNull]
        [ImporterHeader(Name = "Perms")]
        public string Perms { get; set; }
    }
}
