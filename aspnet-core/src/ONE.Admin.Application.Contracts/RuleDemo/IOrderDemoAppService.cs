using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Admin.RuleDemo
{
    public interface IOrderDemoAppService:IApplicationService
    {
        public Task<PagedResult<OrderDemoDto>> QueryAsync(OrderDemoQueryInput input);
    }
}
