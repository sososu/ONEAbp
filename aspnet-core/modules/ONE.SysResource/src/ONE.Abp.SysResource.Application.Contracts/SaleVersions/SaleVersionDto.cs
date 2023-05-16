using System;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionDto:ExtensibleAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get;  set; }
    }
}
