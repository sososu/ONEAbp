using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionCreateInput:ExtensibleObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [DynamicStringLength(typeof(SysResourceConst), nameof(SysResourceConst.MaxNameLength))]
        [Display(Name = "SaleVersionName")]
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [CanBeNull]

        [MaxLength(1024)]
        public string Description { get; set; }
    }

    public class SaleVersionUpdateInput : SaleVersionCreateInput
    {
       
    }
}
