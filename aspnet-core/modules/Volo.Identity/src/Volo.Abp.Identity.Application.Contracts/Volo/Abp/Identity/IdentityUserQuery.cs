
using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;
using System;

namespace Volo.Abp.Identity
{
    public class IdentityUserQuery:PagedQuery
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Query(Compare =QueryCompare.Like)]
        public string UserName { get; set; }


        [Query("PhoneNumber", Compare = QueryCompare.Like)]
        public string PhoneNumber { get; set; }

        [Query("IsActive", Compare = QueryCompare.Equal)]
        public bool? IsActive { get; set; }


        [Query("CreationTime", Compare = QueryCompare.GreaterThanOrEqual)]
        public DateTime? Start { get; set; }


        [Query("CreationTime", Compare = QueryCompare.LessThanOrEqual)]
        public DateTime? End { get; set; }

        public Guid? RoleId { get; set; }

        public Guid? OrganizationUnitId { get; set; }

        public bool IncludeOrganizationUnitChildren { get; set; }=true;
    }
}
