using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public class OrganizationUnitCreateInput:ExtensibleObject
    {
        /// <summary>
        /// Parent <see cref="OrganizationUnitCreateInput"/> Id.
        /// Null, if this OU is a root.
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// Display name of this OrganizationUnit.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public virtual string DisplayName { get; set; }
    }
}
