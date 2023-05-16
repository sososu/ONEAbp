using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Admin.RuleDemo;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace ONE.Admin.Controllers
{
    [Route("api/order-demo")]
    public class OrderDemoCOntroller : AbpController, IOrderDemoAppService
    {
        protected IOrderDemoAppService OrderDemoAppService { get; }
        public OrderDemoCOntroller(IOrderDemoAppService orderDemoAppService) { OrderDemoAppService = orderDemoAppService; }


        [HttpGet("page")]
        public Task<PagedResult<OrderDemoDto>> QueryAsync(OrderDemoQueryInput input)
        {
           return OrderDemoAppService.QueryAsync(input);  
        }
    }
}
