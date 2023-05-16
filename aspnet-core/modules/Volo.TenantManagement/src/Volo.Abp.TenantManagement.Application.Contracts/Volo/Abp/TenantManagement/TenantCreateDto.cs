using System;
using System.Collections.Generic;

namespace Volo.Abp.TenantManagement;

public class TenantCreateDto : TenantCreateOrUpdateDtoBase
{
    public bool IsActive { get; set; }



    public List<ConnectionStringCreateDto> ConnectionStrings { get; set; }

    public TenantInitDataInput InitData { get; set; }

}
