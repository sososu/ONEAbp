using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.Data.Rules;
using ONE.Abp.DataPermission.Tests.Datas;

namespace ONE.Abp.DataPermission.Tests.Tests
{
    public class RuleEngin_Tests : DataPermissionTestBase
    {
        private readonly IRuleEngine _ruleEngine;


        protected override void BeforeAddApplication(IServiceCollection services)
        {
      //      var ruleStore = new Mock<IRuleStore>();
      //      ruleStore
      //      .Setup(m => m.GetUserDataRulesAsync(It.IsAny<string>()))
      //      .Returns(Task.FromResult(RuleData.UserDataRuleResults));


      //      ruleStore
      //     .Setup(m => m.GetUserRuleAsync(It.IsAny<Guid>()))
      //     .Returns(Task.FromResult(RuleData.UserRuleResult));


      //      ruleStore
      //  .Setup(m => m.GetDataRuleAsync(It.Is<Guid>(q=>q== RuleData.DataRuleResult1.Id)))
      //  .Returns(Task.FromResult(RuleData.DataRuleResult1));

      //      ruleStore
      //.Setup(m => m.GetDataRuleAsync(It.Is<Guid>(q => q == RuleData.DataRuleResult2.Id)))
      //.Returns(Task.FromResult(RuleData.DataRuleResult2));

      //      services.AddSingleton<IRuleStore>(ruleStore.Object);
      //      base.BeforeAddApplication(services);
        }

        public RuleEngin_Tests()
        {
            _ruleEngine = GetRequiredService<IRuleEngine>();
        }

        [Fact]
        public async Task Execute_Test()
        {
            var rules = await _ruleEngine.ExecuteAsync<Customer, CustomerDto>();
            Assert.NotNull(rules);
        }

    }
}
