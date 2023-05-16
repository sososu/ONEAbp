using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataDictionary.DataItems
{
    public interface IDataItemAppService:IApplicationService
    {
        public Task<ListResultDto<DataItemDto>> GetEnableCategoryAsync();

        public Task<ListResultDto<DataItemDto>> GetEnableItemsAsync(Guid categoryId);


        public Task<ListResultDto<DataItemDto>> GetCategoryAsync();

        public Task CreateCategoryAsync(DataCategoryCreateInput input);

        public Task UpdateCategoryAsync(Guid id,DataCategoryUpdate input);


        public Task CreateAsync(DataItemCreateInput input);

        public Task UpdateAsync(Guid id, DataItemCreateUpdate input);


        public Task DeleteAsync(Guid id);

        public Task<ListResultDto<DataItemDto>> GetItemsAsync(Guid categoryId);

        public Task<ListResultDto<DataItemDto>> GetItemsByParentCodeAsync(string code);
    }
}
