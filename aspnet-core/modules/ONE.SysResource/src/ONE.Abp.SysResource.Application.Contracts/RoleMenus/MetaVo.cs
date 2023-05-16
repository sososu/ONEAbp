using ONE.Abp.Shared.Utils;

namespace ONE.Abp.SysResource.RoleMenus
{
    /// <summary>
    /// 路由显示信息
    /// </summary>
    public class MetaVo
    {
        /// <summary>
        /// 设置该路由在侧边栏和面包屑中展示的名字
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置该路由的图标，对应路径src/assets/icons/svg
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 设置为true，则不会被 <keep-alive>缓存
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// 内链地址（http(s)://开头）
        /// </summary>
        public string Link { get; set; }

        public MetaVo()
        {
        }

        public MetaVo(string title, string icon)
        {
            Title = title;
            Icon = icon;
        }

        public MetaVo(string title, string icon, bool noCache)
        {
            Title = title;
            Icon = icon;
            NoCache = noCache;
        }

        public MetaVo(string title, string icon, string link)
        {
            Title = title;
            Icon = icon;
            Link = link;
        }

        public MetaVo(string title, string icon, bool noCache, string link)
        {
            Title = title;
            Icon = icon;
            NoCache = noCache;
            if(link.IsUrl())
                Link = link;
        }
    }
}
