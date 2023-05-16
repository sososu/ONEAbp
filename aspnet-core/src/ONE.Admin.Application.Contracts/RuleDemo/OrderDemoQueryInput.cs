using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Admin.RuleDemo
{
    public class OrderDemoQueryInput:PagedQuery
    {
        [Query(Compare =QueryCompare.Like)]
        public string ProductName { get; set; }
    }
}
