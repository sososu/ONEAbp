using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ONE.Abp.SysResource.SysMenus
{
    public class SysMenuQuery:PagedQuery
    {
        [Query(Compare = QueryCompare.Like)]
        public string Name { get; protected set; }

        [Query(Compare =QueryCompare.Like)]
        public string Code { get; protected set; }
    }
}
