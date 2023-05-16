using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace ONE.Abp.DataDictionary.DataItems
{
    public class EnumDicDataSeed_Tests : DataDictionaryDomainTestBase
    {
        //private readonly SampleManager _sampleManager;

        public EnumDicDataSeed_Tests()
        {
            //_sampleManager = GetRequiredService<SampleManager>();
        }

        [Fact]
        public async Task GetSeedAsync_Test()
        {
            var repository = ServiceProvider.GetService<IRepository<DataItem, Guid>>();
            var list = await repository.GetListAsync();
            list.ShouldNotBeNull();
        }
    }
}
