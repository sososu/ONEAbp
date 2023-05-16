using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionMenuEditInput
    {
        [Required]
        public Guid AppId { get; set; }

        [Required]
        /// <summary>
        /// 菜单编码
        /// </summary>
        public IList<string> MenuCodes { get; set; }
    }
}
