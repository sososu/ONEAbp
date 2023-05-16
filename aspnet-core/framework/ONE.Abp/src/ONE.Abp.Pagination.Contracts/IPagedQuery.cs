using ONE.Abp.Pagination.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ONE.Abp.Pagination.Contracts
{
    /// <summary>
    /// 定义分页查询
    /// </summary>
    public interface IPagedQuery : IPagedSortInfo, IQuery
    {

    }
}
