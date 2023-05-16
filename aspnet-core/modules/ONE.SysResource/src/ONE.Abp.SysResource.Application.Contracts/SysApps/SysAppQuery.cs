using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Abp.SysResource.SysApps
{
    public class SysAppQuery:PagedQuery
    {
        [Query(Compare =QueryCompare.Like)]
        public string AppName { get; set; }

        [Query(Compare = QueryCompare.Like)]
        public string AppCode { get; set; }

    }
}
