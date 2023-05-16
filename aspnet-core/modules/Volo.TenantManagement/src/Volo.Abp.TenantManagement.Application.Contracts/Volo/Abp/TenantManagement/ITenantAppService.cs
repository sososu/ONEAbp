using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TenantManagement;

public interface ITenantAppService : ICrudAppService<TenantDto, Guid, GetTenantsInput, TenantCreateDto, TenantUpdateDto>
{

    Task<PagedResult<TenantDto>> QueryAsync(TenantQueryInput input);
    Task<string> GetDefaultConnectionStringAsync(Guid id);

    Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString);

    Task DeleteDefaultConnectionStringAsync(Guid id);

    Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringsAsync(Guid id);

    Task UpdateConnectionStringAsync(Guid id, ConnectionStringCreateDto connectionCreateDto);

    Task SetActiveAsync(Guid id, bool isActive);

    Task SetSaleVersionAsync(Guid id, Guid saleVersionId);

    Task DeleteConnectionStringAsync(Guid id);

    Task SetSharedConnectionStringAsync(Guid id);

    Task InitData(Guid id);
}
