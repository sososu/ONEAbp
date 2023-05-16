using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace Volo.Abp.Identity
{
    public class OrganizationUnitQuery:PagedQuery
    {
        /// <summary>
        /// Display name of this OrganizationUnit.
        /// </summary>
        [Query("DisplayName", Compare =QueryCompare.Like)]
        public  string DisplayName { get; set; }
    }
}
