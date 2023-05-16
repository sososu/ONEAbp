using ONE.Abp.DataDictionary.DataItems;
using ONE.Abp.DataPermission.Rules;
using ONE.Abp.FileManagement.Files;
using ONE.Abp.SysResource.RoleMenus;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;

namespace ONE.Admin.HttpApi.Client.ConsoleTestApp;

public class ClientDemoService : ITransientDependency
{
    //account
    private readonly IProfileAppService _profileAppService;

    //identity
    private readonly IIdentityUserAppService _userAppService;
    private readonly IIdentityRoleAppService _roleAppService;
    private readonly IIdentityUserLookupAppService _userLookUpAppService;
    private readonly IOrganizationUnitAppService _organizationUnitAppService;

    //tenant
    private readonly ITenantAppService _tenantAppService;

    //dataDictionary
    private readonly IDataItemAppService _dataItemAppService;

    //dataPermission
    private readonly IDataRuleAppService _dataRuleAppService;
    private readonly IUserRuleAppService _userRuleAppService;
    private readonly IUserDataRuleAppService _userDataRuleAppService;
    private readonly IDataTargetAppService _dataTargetAppService;


    //fileManagement
    private readonly IFileAppService _fileAppService;

    //sysResource
    private readonly ISysAppAppService _sysAppAppService;
    private readonly ISysMenuAppService _sysMenuAppService;


    public ClientDemoService(IProfileAppService profileAppService,
        IIdentityUserAppService userAppService,
        IIdentityRoleAppService roleAppService,
    IIdentityUserLookupAppService userLookUpAppService,
    IOrganizationUnitAppService organizationUnitAppService,
    ITenantAppService tenantAppService,
    IDataItemAppService dataItemAppService,
    IDataRuleAppService dataRuleAppService,
    IUserRuleAppService userRuleAppService,
    IUserDataRuleAppService userDataRuleAppService,
    IDataTargetAppService dataTargetAppService,
    IFileAppService fileAppService,
    ISysAppAppService sysAppAppService,
    ISysMenuAppService sysMenuAppService
        )
    {
        _profileAppService = profileAppService;
        _userAppService = userAppService;
        _roleAppService = roleAppService;
        _userLookUpAppService = userLookUpAppService;
        _organizationUnitAppService = organizationUnitAppService;
        _tenantAppService = tenantAppService;
        _dataItemAppService = dataItemAppService;
        _dataRuleAppService = dataRuleAppService;
        _userRuleAppService = userRuleAppService;
        _userDataRuleAppService = userDataRuleAppService;
        _dataTargetAppService = dataTargetAppService;
        _fileAppService = fileAppService;
        _sysAppAppService = sysAppAppService;
        _sysMenuAppService = sysMenuAppService;
    }

    public async Task RunAsync()
    {
        Console.WriteLine($"Service :[_profileAppService] Method:[GetAsync]");
        var output = await _profileAppService.GetAsync();
        Console.WriteLine($"UserName : {output.UserName}");
        Console.WriteLine($"Email    : {output.Email}");
        Console.WriteLine($"Name     : {output.Name}");
        Console.WriteLine($"Surname  : {output.Surname}");

        Console.WriteLine();
        Console.WriteLine($"Service :[_userAppService]  Method:[QueryAsync,GetAsync]");
        var users = await _userAppService.QueryAsync(new IdentityUserQuery { PageIndex = 1, PageSize = 10 });

        var userExt = users.Items.FirstOrDefault();
        var user = await _userAppService.GetAsync(userExt.Id);
        Console.WriteLine($"UserName : {user.UserName}");

        Console.WriteLine();
        Console.WriteLine($"Service :[_roleAppService]  Method:[QueryAsync,GetAsync]");

        var roles = await _roleAppService.QueryAsync(new QueryRoleInput { PageIndex = 1, PageSize = 10, });
        var role = roles.Items.FirstOrDefault();
        role = await _roleAppService.GetAsync(role.Id);
        Console.WriteLine($"RoleName : {role.Name}");


        Console.WriteLine();
        Console.WriteLine($"Service :[_userLookUpAppService]  Method:[FindByUserNameAsync,FindByIdAsync，SearchAsync]");

        var usedata = await _userLookUpAppService.FindByUserNameAsync(user.UserName);
        usedata = await _userLookUpAppService.FindByIdAsync(user.Id);
        var usedatas = await _userLookUpAppService.SearchAsync(new UserLookupSearchInputDto());

        Console.WriteLine($"UserName : {usedata.Name}");


        Console.WriteLine();
        Console.WriteLine($"Service :[_organizationUnitAppService]  Method:[GetListAsync]");
        var orgs = await _organizationUnitAppService.GetListAsync();


        Console.WriteLine();
        Console.WriteLine($"Service :[_tenantAppService]  Method:[GetListAsync]");
        var tenants = await _tenantAppService.GetListAsync(new GetTenantsInput());

        Console.WriteLine();
        Console.WriteLine($"Service :[_dataItemAppService]  Method:[GetCategoryAsync,GetItemsAsync]");
        var categorys = await _dataItemAppService.GetCategoryAsync();
        var category = categorys.Items.FirstOrDefault();
        var items = await _dataItemAppService.GetItemsAsync(category.Id);
        Console.WriteLine($"category : {category.Name}");


        Console.WriteLine();
        Console.WriteLine($"Service :[_dataTargetAppService]  Method:[GetListAsync,GetAsync]");
        var targets = await _dataTargetAppService.GetListAsync();
        var target = targets.Items.FirstOrDefault();
        target = await _dataTargetAppService.GetAsync(target.Name);
        Console.WriteLine($"target : {target.Name}");


        Console.WriteLine();
        Console.WriteLine($"Service :[_dataRuleAppService]  Method:[GetIdNamesAsync]");
        var drnames = await _dataRuleAppService.GetIdNamesAsync(target.Name);


        Console.WriteLine();
        Console.WriteLine($"Service :[_userRuleAppService]  Method:[GetIdNamesAsync]");
        var urnnames = await _userRuleAppService.GetIdNamesAsync();

        Console.WriteLine();
        Console.WriteLine($"Service :[_userDataRuleAppService]  Method:[GetRules]");
        var rules = await _userDataRuleAppService.GetRules(target.Name);

        Console.WriteLine();
        Console.WriteLine($"Service :[_sysAppAppService]  Method:[GetAsync]");
        var app = await _sysAppAppService.GetAsync(Guid.Parse("3a08cbf7-7321-dd8b-91f6-b4761aa85c0d"));

        Console.WriteLine();
        Console.WriteLine($"Service :[_sysMenuAppService]  Method:[GetListByAppCodeAsync]");
        var menus = await _sysMenuAppService.GetListByAppCodeAsync(app.AppCode);

        Console.WriteLine();
        Console.WriteLine($"Service :[_fileAppService]  Method:[GetStatisticsAsync]");
        var statisticsDto = await _fileAppService.GetStatisticsAsync();


        //POST 请求

        //var roles = await _roleAppService.CreateAsync(new IdentityRoleCreateDto { Name="cc"});


        //string str = "我是谁";
        //byte[] bytesArray = Encoding.UTF8.GetBytes(str);
        //using (MemoryStream ms = new MemoryStream(bytesArray))
        //{
        //   await _fileAppService.CreateAsync(new FileUploadInputDto
        //    {
        //        File = new RemoteStreamContent(ms, "ts.txt", "text/plain"),
        //        Tag = "test"
        //    });

        //}


    }
}
