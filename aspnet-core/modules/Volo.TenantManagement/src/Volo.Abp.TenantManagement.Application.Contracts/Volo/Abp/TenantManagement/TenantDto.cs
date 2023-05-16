using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TenantManagement;

public class TenantDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public string Contact { get; set; }
    public string ContactWay { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public Guid? SaleVersionId { get; set; }

    public string ConcurrencyStamp { get; set; }
}
