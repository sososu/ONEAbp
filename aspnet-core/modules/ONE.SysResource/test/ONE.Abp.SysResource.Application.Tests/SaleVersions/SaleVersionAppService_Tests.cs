using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionAppService_Tests : SysResourceApplicationTestBase
    {
        private readonly ISaleVersionAppService _appService;

        public SaleVersionAppService_Tests()
        {
            _appService = GetRequiredService<ISaleVersionAppService>();
        }


        [Fact]
        public async Task Test()
        {
           //await _appService.GetAppsAysnc();
        }
    }
}
