using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.Abp.TenantManagement;

public abstract class TenantCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]
    [Display(Name = "TenantName")]
    public string Name { get; set; }

    [MaxLength(64)]
    public string Contact { get; set; }

    [MaxLength(256)]
    public string ContactWay { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public Guid? SaleVersionId { get; set; }
    public TenantCreateOrUpdateDtoBase() : base(false)
    {

    }
}
