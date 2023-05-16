using JetBrains.Annotations;
using ONE.Abp.SysResource.Menus;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace ONE.Abp.SysResource.SysMenus
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenu : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get; protected set; }


        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// 父级菜单编码
        /// </summary>
        public string ParentCode { get; protected set; }


        /// <summary>
        /// 显示顺序 从小到大
        /// </summary>
        public int Order { get; protected set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [CanBeNull]
        public string Path { get; protected set; }


        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; protected set; }


        /// <summary>
        /// 路由参数
        /// </summary>
        [CanBeNull]
        public string Query { get; protected set; }


        /// <summary>
        ///  是否为外链
        /// </summary>
        public bool IsFrame { get; protected set; }


        /// <summary>
        ///  是否缓存
        /// </summary>
        public bool IsCache { get; protected set; }

        /// <summary>
        /// 是否启用 默认True
        /// </summary>
        public bool IsEnable { get; protected set; }


        /// <summary>
        /// 显示状态 默认True
        /// </summary>
        public bool Visible { get; protected set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [CanBeNull]
        public string Icon { get; protected set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MenuType MenuType { get; protected set; }


        /// <summary>
        /// 系统应用Id
        /// </summary>
        public Guid SysAppId { get; protected set; }


        /// <summary>
        /// 权限字符串
        /// </summary>
        [CanBeNull]
        public string Perms { get; protected set; }

        protected SysMenu()
        {

        }

        public SysMenu(Guid id, Guid sysAppId,string code)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNull(sysAppId, nameof(sysAppId));

            SysAppId = sysAppId;
            Id = id;
            SetCode(code); 
        }

        public void SetCode(string code)
        {
            Check.NotNull(code, nameof(code));
            Code = code;
        }

        public void SetBasicInfo(string name, int order, string path, string component, MenuType type, string parentCode = null, string icon = null, string query = null)
        {
            Name = name;
            Order = order;
            Path = path;
            Component = component;
            MenuType = type;
            ParentCode = parentCode;
            Icon = icon;
            Query = query;
        }


        public void SetPerms(string perms)
        {
            Perms = perms;
        }

        public void SetEnable(bool isFrame, bool isCache, bool isEnable, bool visible)
        {
            IsFrame = isFrame;
            IsCache = isCache;
            IsEnable = isEnable;
            Visible = visible;
        }
    }
}
