using Microsoft.AspNetCore.Authorization;
using ONE.Abp.Shared;
using ONE.Abp.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using ONE.Abp.Pagination.Contracts.Dtos;

namespace Volo.Abp.TenantManagement;

[Authorize(TenantManagementPermissions.Tenants.Default)]
public class TenantAppService : TenantManagementAppServiceBase, ITenantAppService
{
    protected IDataSeeder DataSeeder { get; }
    protected ITenantRepository TenantRepository { get; }
    protected ITenantManager TenantManager { get; }
    protected IDistributedEventBus DistributedEventBus { get; }

    public TenantAppService(
        ITenantRepository tenantRepository,
        ITenantManager tenantManager,
        IDataSeeder dataSeeder,
        IDistributedEventBus distributedEventBus)
    {
        DataSeeder = dataSeeder;
        TenantRepository = tenantRepository;
        TenantManager = tenantManager;
        DistributedEventBus = distributedEventBus;
    }


    public virtual async Task<TenantDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Tenant, TenantDto>(
            await TenantRepository.GetAsync(id)
        );
    }

    public virtual async Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantsInput input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Tenant.Name);
        }

        var count = await TenantRepository.GetCountAsync(input.Filter);
        var list = await TenantRepository.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.Filter
        );

        return new PagedResultDto<TenantDto>(
            count,
            ObjectMapper.Map<List<Tenant>, List<TenantDto>>(list)
        );
    }

    public virtual async Task<PagedResult<TenantDto>> QueryAsync(TenantQueryInput input)
    {
        return await (await TenantRepository.WithDetailsAsync()).ToPagedResultAsync<Tenant, TenantDto>(input);
    }

    [Authorize(TenantManagementPermissions.Tenants.Create)]
    public virtual async Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        var tenant = await TenantManager.CreateAsync(input.Name);
        tenant.SetIsActive(true);
        input.MapExtraPropertiesTo(tenant);
        tenant.SetContact(input.Contact, input.ContactWay);
        if (input.ExpirationDate.HasValue)
            tenant.SetExpirationDate(input.ExpirationDate.Value);
        if (input.SaleVersionId.HasValue)
            tenant.SetSaleVersionId(input.SaleVersionId.Value);

        if (input.ConnectionStrings != null && input.ConnectionStrings.Any())
        {
            foreach (var connectionString in input.ConnectionStrings)
            {
                tenant.SetConnectionString(connectionString.Name, connectionString.Value);
            }
        }

        await TenantRepository.InsertAsync(tenant);
        await CurrentUnitOfWork.SaveChangesAsync();

        await PublishTenantCreateEvent(tenant.Id, input.InitData);

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    [Authorize(TenantManagementPermissions.Tenants.Update)]
    public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        var tenant = await TenantRepository.GetAsync(id);

        await TenantManager.ChangeNameAsync(tenant, input.Name);
        tenant.SetContact(input.Contact, input.ContactWay);
        if (input.ExpirationDate.HasValue)
            tenant.SetExpirationDate(input.ExpirationDate.Value);

        tenant.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        input.MapExtraPropertiesTo(tenant);


        var changeSaleVersion = false;
        if (input.SaleVersionId.HasValue)
        {
            changeSaleVersion = input.SaleVersionId != tenant.SaleVersionId;
            tenant.SetSaleVersionId(input.SaleVersionId.Value);
        }

        await TenantRepository.UpdateAsync(tenant);
        await CurrentUnitOfWork.SaveChangesAsync();

        if (changeSaleVersion)
        {
            await DistributedEventBus.PublishAsync(
      new TenantCreatedEto
      {
          Id = tenant.Id,
          Name = tenant.Name,
          Properties =
          {
                {"SaleVersionId",input.SaleVersionId.ToString()}
          }
      });
        }

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    [Authorize(TenantManagementPermissions.Tenants.Update)]
    public virtual async Task SetSaleVersionAsync(Guid id, Guid saleVersionId)
    {
        var tenant = await TenantRepository.GetAsync(id);

        tenant.SetSaleVersionId(saleVersionId);
        await TenantRepository.UpdateAsync(tenant);

        await DistributedEventBus.PublishAsync(
       new TenantCreatedEto
       {
           Id = tenant.Id,
           Name = tenant.Name,
           Properties =
           {
                {"SaleVersionId",tenant.SaleVersionId.ToString()}
           }
       });
    }

    [Authorize(TenantManagementPermissions.Tenants.Enable)]
    public virtual async Task SetActiveAsync(Guid id, bool isActive)
    {
        var tenant = await TenantRepository.GetAsync(id);

        tenant.SetIsActive(isActive);
        await TenantRepository.UpdateAsync(tenant);
    }


    [Authorize(TenantManagementPermissions.Tenants.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
        {
            return;
        }

        await TenantRepository.DeleteAsync(tenant);
    }

    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task<string> GetDefaultConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        return tenant?.FindDefaultConnectionString();
    }

    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
    {
        var tenant = await TenantRepository.GetAsync(id);
        tenant.SetDefaultConnectionString(defaultConnectionString);
        await TenantRepository.UpdateAsync(tenant);
    }

    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task DeleteDefaultConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        tenant.RemoveDefaultConnectionString();
        await TenantRepository.UpdateAsync(tenant);
    }


    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringsAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        return new ListResultDto<TenantConnectionStringDto>(ObjectMapper.Map<List<TenantConnectionString>, List<TenantConnectionStringDto>>(tenant.ConnectionStrings));
    }

    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task UpdateConnectionStringAsync(Guid id, ConnectionStringCreateDto connectionCreateDto)
    {
        var tenant = await TenantRepository.GetAsync(id);
        tenant.SetConnectionString(connectionCreateDto.Name, connectionCreateDto.Value);
        await TenantRepository.UpdateAsync(tenant);
    }

    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task DeleteConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        tenant.RemoveDefaultConnectionString();
        await TenantRepository.UpdateAsync(tenant);
    }

    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task SetSharedConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        tenant.RemoveAllConnectionString();
        await TenantRepository.UpdateAsync(tenant);
    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public virtual Task InitData(Guid id)
    {
        return PublishTenantCreateEvent(id);
    }

    private async Task PublishTenantCreateEvent(Guid id, TenantInitDataInput input = null)
    {
        var tenant = await TenantRepository.GetAsync(id);
        if (!tenant.SaleVersionId.HasValue)
            throw new BusinessException("Volo.Abp.TenantManagement:NotSelectSaleVersion");

        //迁移交给专业的DB迁移，不在接口实现（安全问题）
        if (input == null)
        {
            await DistributedEventBus.PublishAsync(
         new TenantCreatedEto
         {
             Id = tenant.Id,
             Name = tenant.Name,
             Properties =
             {
                {"SaleVersionId",tenant.SaleVersionId.ToString()}
             }
         });
        }
        else
        {
            await DistributedEventBus.PublishAsync(
         new TenantCreatedEto
         {
             Id = tenant.Id,
             Name = tenant.Name,
             Properties =
             {
                        { "AdminEmail", input.AdminEmailAddress },
                        { "AdminPassword", input.AdminPassword },
                        {"SaleVersionId",tenant.SaleVersionId.ToString()}
             }
         });

            using (CurrentTenant.Change(tenant.Id, tenant.Name))
            {
                await DataSeeder.SeedAsync(
                                new DataSeedContext(tenant.Id)
                                    .WithProperty("AdminEmail", input.AdminEmailAddress)
                                    .WithProperty("AdminPassword", input.AdminPassword)
                                );
            }
        }
    }
}
