using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class OrganizationUnitUpdateInput: OrganizationUnitCreateInput
    {
        [Required]
        public Guid Id { get; set; }
    }
}
