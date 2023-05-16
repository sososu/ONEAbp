using ONE.Abp.DataPermission.Extension;
using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ONE.Admin.RuleDemo
{
    public class OrderDemoAppService : AdminAppService, IOrderDemoAppService
    {
        protected IRepository<OrderDemo> Repository;
        public OrderDemoAppService(IRepository<OrderDemo> repository) { Repository = repository; }

        public async Task<PagedResult<OrderDemoDto>> QueryAsync(OrderDemoQueryInput input)
        {
            return await(await Repository.WithDetailsAsync()).ToPagedResultForRuleAsync<OrderDemo, OrderDemoDto>(input);
        }
    }
}
