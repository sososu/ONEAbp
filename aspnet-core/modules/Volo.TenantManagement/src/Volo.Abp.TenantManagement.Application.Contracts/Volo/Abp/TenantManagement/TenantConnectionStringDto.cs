using System;

namespace Volo.Abp.TenantManagement
{
    public class TenantConnectionStringDto
    {
        public Guid TenantId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
