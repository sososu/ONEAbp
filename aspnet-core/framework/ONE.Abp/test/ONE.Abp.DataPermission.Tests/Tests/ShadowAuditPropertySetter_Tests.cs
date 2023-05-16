using ONE.Abp.DataPermission.Tests.Datas;
using Shouldly;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.DataPermission.Tests.Tests
{
    public class ShadowAuditPropertySetter_Tests : DataPermissionTestBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public ShadowAuditPropertySetter_Tests()
        {
            _customerRepository = GetRequiredService<IRepository<Customer>>();
        }

        [Fact]
        public async Task SetCreationProperties_Tests()
        {

            var customer = new Customer(Guid.NewGuid())
            {
                Name = "A",
                Address = new Address("中国", "深圳", "南山区", "68255", "宝安区新安一路69号"),
                Mobile = "12546584541",
                Teams = new List<Team>
                      {
                          new Team {Name="Ta",Mobile="1551552"}
                      },
            };

          var c= await _customerRepository.InsertAsync(customer);

          c.OrganizationCode.ShouldNotBeNull();

            var customer2 = new Customer(Guid.NewGuid())
            {
                Name = "B",
                Address = new Address("中国", "深圳", "南山区", "68255", "宝安区新安一路69号"),
                Mobile = "12546584541",
                Teams = new List<Team>
                      {
                          new Team {Name="Ta",Mobile="1551552"}
                      },
                OrganizationCode = "c",
                CreatorId = Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d")
            };

            c = await _customerRepository.InsertAsync(customer2);
            c.OrganizationCode.ShouldBe("c");
        }
    }
}
