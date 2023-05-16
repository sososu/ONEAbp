using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace MyCompanyName.MyProjectName.Samples;

/* This is just an example test class.
 * Normally, you don't test code of the modules you are using
 * (like IdentityUserManager here).
 * Only test your own domain services.
 */
[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class SampleDomainTests : MyProjectNameDomainTestBase
{
  

    public SampleDomainTests()
    {
       
    }

    [Fact]
    public async Task Should_Set_Email_Of_A_User()
    {
     
    }
}
