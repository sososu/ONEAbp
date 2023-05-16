using ONE.Abp.SysResource.SysApps;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace MyCompanyName.MyProjectName.Data
{
    public class SysAppSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<SysApp> _repository;
        public SysAppSeedContributor(IRepository<SysApp> repository)
        {
            _repository = repository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _repository.CountAsync() > 0) return;


            var app = new SysApp(Guid.Parse("3a08cbf7-7321-dd8b-91f6-b4761aa85c0d"), "idscode");
            app.SetBasicInfo("后台管理中心", "v1.0.0");
            app.SetDescription("后台管理中心");

            await _repository.InsertAsync(app);

        }
    }
}
