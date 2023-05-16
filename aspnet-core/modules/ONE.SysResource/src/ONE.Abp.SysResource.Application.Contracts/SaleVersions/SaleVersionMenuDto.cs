using System;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionMenuDto
    {
        public Guid SaleVersonId { get; set; }

        public Guid AppId { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }
    }
}
