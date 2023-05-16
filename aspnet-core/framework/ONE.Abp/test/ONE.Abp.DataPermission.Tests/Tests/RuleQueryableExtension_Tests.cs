using Microsoft.EntityFrameworkCore;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Data.Rules;
using ONE.Abp.DataPermission.Extension;
using ONE.Abp.DataPermission.Tests.Datas;
using ONE.Abp.Pagination;
using Shouldly;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.DataPermission.Tests.Tests
{
    public class RuleQueryableExtension_Tests : DataPermissionTestBase
    {

        private readonly IRepository<Customer> _customerRepository;

        public RuleQueryableExtension_Tests()
        {
            _customerRepository = GetRequiredService<IRepository<Customer>>();
        }

        [Fact]
        public async Task CreateFilterUserConditionExpression_Tests()
        {
            await SeedDataAsync();

            var input = new CustomerQuery { PageIndex = 1, PageSize = 10, SortFields = new List<string> { "Name Asc" } };

            ServiceContainer.Set(ServiceProvider);
            var list =await WithUnitOfWorkAsync<PagedResult<CustomerDto>>(async () =>
            {
               return await (await _customerRepository.WithDetailsAsync()).Include(c => c.Teams).ToPagedResultForRuleAsync<Customer, CustomerDto>(input);
            });

           
            Assert.NotNull(list);

            list.TotalCount.ShouldBe(2);

            list.Items[0].Name.ShouldBe(CustomerData.CustomerA.Name);
            list.Items[0].MapAddress.Detial.ShouldBeNull();
            list.Items[0].MapAddress.Detial.ShouldBeNull();
            list.Items[0].ExtraProperties[RuleDataOperationNameConst.CanEdit].ShouldBe(true);
            list.Items[0].ExtraProperties[RuleDataOperationNameConst.CanRemove].ShouldBe(true);

            list.Items[1].OrganizationCode.ShouldBe(CustomerData.CustomerB.OrganizationCode);
            list.Items[1].Mobile.ShouldBeNull();
            list.Items[1].Name.ShouldBeNull();
            list.Items[1].ExtraProperties[RuleDataOperationNameConst.CanEdit].ShouldBe(false);
            list.Items[1].ExtraProperties[RuleDataOperationNameConst.CanRemove].ShouldBe(false);

        }


        private async Task SeedDataAsync()
        {
            await _customerRepository.InsertManyAsync(CustomerData.Customers);
        }

    }

    public class CustomerQuery : PagedQuery
    {

    }

}
