using ONE.Abp.SysResource.Datas;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionManager_Tests : SysResourceDomainTestBase
    {
        private readonly ISaleVersionManager _saleVersionManager;

        public SaleVersionManager_Tests()
        {
            _saleVersionManager = GetRequiredService<ISaleVersionManager>();
        }

        [Fact]
        public async Task GetAppsAsync_Test()
        {
            var result=await _saleVersionManager.GetAppsAsync(SaleVersionData.SaleVersionA.Id);
            result.ShouldNotBeNull();
        }
    }
}
