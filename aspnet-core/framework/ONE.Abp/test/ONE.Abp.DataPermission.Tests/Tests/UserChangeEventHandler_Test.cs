using ONE.Abp.Data.Rules;
using ONE.Abp.DataPermission.Tests.Datas;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission.Tests.Tests
{
    public class UserChangeEventHandler_Test : DataPermissionTestBase
    {

        private readonly IRepository<Customer> _customerRepository;

        public UserChangeEventHandler_Test()
        {
            _customerRepository = GetRequiredService<IRepository<Customer>>();
        }

        [Fact]
        public async Task HandleEventAsync_Tests()
        {

            await InitDataAsync();

            var user = new UserEto { Id = Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d"), ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary { { "OrganizationCode", "d" } }};

            var handler = (UserChangeEventHandler)ServiceProvider.GetService(typeof(UserChangeEventHandler));
            if (handler == null)
                return;

            await handler.HandleEventAsync(new EntityUpdatedEto<UserEto>(user));

            var entities = await _customerRepository.GetListAsync(x=>x.CreatorId== user.Id);

            Assert.NotNull(entities);
            entities.Count.ShouldBe(2);
            entities[0].OrganizationCode.ShouldBe("d");
            entities[1].OrganizationCode.ShouldBe("d");
        }

        private async Task InitDataAsync()
        {
            await _customerRepository.InsertManyAsync(CustomerData.Customers);
        }
    }
}
