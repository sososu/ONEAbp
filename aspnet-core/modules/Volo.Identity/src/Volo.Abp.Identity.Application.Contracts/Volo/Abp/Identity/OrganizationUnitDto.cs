using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class OrganizationUnitDto:ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// Parent <see cref="OrganizationUnitDto"/> Id.
        /// Null, if this OU is a root.
        /// </summary>
        public virtual Guid? ParentId { get; internal set; }

        /// <summary>
        /// Hierarchical Code of this organization unit.
        /// Example: "00001.00042.00005".
        /// This is a unique code for an OrganizationUnit.
        /// It's changeable if OU hierarchy is changed.
        /// </summary>
        public virtual string Code { get; internal set; }

        /// <summary>
        /// Display name of this OrganizationUnit.
        /// </summary>
        public virtual string DisplayName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
