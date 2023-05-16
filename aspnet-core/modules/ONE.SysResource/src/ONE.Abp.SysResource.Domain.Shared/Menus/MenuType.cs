using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace ONE.Abp.SysResource.Menus
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 目录
        /// </summary>
        [Display(Name = "目录")]
        M = 0,

        /// <summary>
        /// 菜单
        /// </summary>
        [Display(Name = "菜单")]
        C = 1,

        /// <summary>
        /// 按钮
        /// </summary>
        [Display(Name = "按钮")]
        F = 2
    }
}
