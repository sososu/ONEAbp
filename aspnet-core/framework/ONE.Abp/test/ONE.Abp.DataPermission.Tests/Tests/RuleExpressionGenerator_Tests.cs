using ONE.Abp.Data.Rules;
using ONE.Abp.DataPermission.Tests.Datas;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission.Tests.Tests
{
    public class RuleExpressionGenerator_Tests : DataPermissionTestBase
    {

        private readonly IRuleExpressionGenerator _ruleExpressionGenerator;

        public RuleExpressionGenerator_Tests()
        {
            _ruleExpressionGenerator = GetRequiredService<IRuleExpressionGenerator>();
        }

        [Fact]
        public async Task CreateFilterUserConditionExpression_Tests()
        {
            var expression = _ruleExpressionGenerator.CreateFilterUserConditionExpression<ICurrentUser>(RuleData.UserRuleResult.Conditions);
            Assert.NotNull(expression);

            var func= expression.Compile();

            var user = GetRequiredService<ICurrentUser>();
            var bl= func.Invoke(user);
            Assert.True(bl);
        }


        [Fact]
        public async Task CreateFilterConditionExpression1_Tests()
        {
            var expression = _ruleExpressionGenerator.CreateFilterConditionExpression<Customer>(RuleData.DataRuleResult1.Conditions);
            Assert.NotNull(expression);

            var func = expression.Compile();

            var bl = func.Invoke(CustomerData.CustomerA);
            Assert.True(bl);

            bl = func.Invoke(CustomerData.CustomerB);
            Assert.False(bl);

            bl = func.Invoke(CustomerData.CustomerC);
            Assert.False(bl);
        }

        [Fact]
        public async Task CreateFilterConditionExpression2_Tests()
        {
            var expression = _ruleExpressionGenerator.CreateFilterConditionExpression<Customer>(RuleData.DataRuleResult2.Conditions);
            Assert.NotNull(expression);

            var func = expression.Compile();

            var bl = func.Invoke(CustomerData.CustomerA);
            Assert.True(bl);

            bl = func.Invoke(CustomerData.CustomerB);
            Assert.True(bl);

            bl = func.Invoke(CustomerData.CustomerC);
            Assert.False(bl);
        }


        [Fact]
        public async Task BuildResetPropertiesExpression_Tests()
        {
            var action = _ruleExpressionGenerator.BuildResetPropertiesExpression<Customer>(RuleData.DataRuleResult1.HideDataTargetFields.Split(","));
            Assert.NotNull(action);


            var customer=new Customer(Guid.NewGuid())
            {
                Name = "A",
                Address = new Address("中国", "深圳", "南山区", "68255", "宝安区新安一路69号"),
                Mobile = "12546584541",
                Teams = new List<Team>
                      {
                          new Team {Name="Ta",Mobile="1551552"}
                      },
                OrganizationCode = "V",
                CreatorId = Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d")
            };

            Assert.NotNull(customer.Address.Detial);
            Assert.NotNull(customer.Address.City);

            action.Invoke(customer);
            Assert.Null(customer.Address.Detial);
            Assert.Null(customer.Address.City);
        }


        [Fact]
        public async Task BuildResetPropertiesExpression2_Tests()
        {
            var action = _ruleExpressionGenerator.BuildResetPropertiesExpression<CustomerDto>(RuleData.DataRuleResult1.HideDataTargetFields.Split(","));
            Assert.NotNull(action);
            CustomerDto customer = new CustomerDto
            {
                MapAddress = new AddressDto { Detial = "xfsdfsfxxfd", MapCity = "cdfsdfsdfsdfsdfs" }
            };

            Assert.NotNull(customer.MapAddress.Detial);
            Assert.NotNull(customer.MapAddress.MapCity);

            action.Invoke(customer);
            Assert.Null(customer.MapAddress.Detial);
            Assert.Null(customer.MapAddress.MapCity);
        }
    }
}
