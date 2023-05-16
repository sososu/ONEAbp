using Microsoft.AspNetCore.Authorization;
using ONE.Abp.DataDictionary.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.DataDictionary.DataItems
{
    [Authorize]
    public class DataItemAppService : DataDictionaryAppService, IDataItemAppService
    {
        private readonly IRepository<DataItem, Guid> _repository;
        private readonly IDataItemManager _dataItemManager;
        public DataItemAppService(IRepository<DataItem, Guid> repository, IDataItemManager dataItemManager) { _repository = repository; _dataItemManager = dataItemManager; }

        public async Task<ListResultDto<DataItemDto>> GetEnableCategoryAsync()
        {
            return new ListResultDto<DataItemDto>(ObjectMapper.Map<List<DataItem>, List<DataItemDto>>(await _repository.GetListAsync(r => !r.ParentId.HasValue && r.Status==DataItemStatus.Enable)));
        }

        public async Task<ListResultDto<DataItemDto>> GetEnableItemsAsync(Guid categoryId)
        {
            return new ListResultDto<DataItemDto>(ObjectMapper.Map<List<DataItem>, List<DataItemDto>>(await _repository.GetListAsync(r => r.ParentId == categoryId && r.Status == DataItemStatus.Enable)));
        }

        public async Task<ListResultDto<DataItemDto>> GetCategoryAsync()
        {
            return new ListResultDto<DataItemDto>(ObjectMapper.Map<List<DataItem>, List<DataItemDto>>(await _repository.GetListAsync(r => !r.ParentId.HasValue)));
        }


       [Authorize(Policy = DataDictionaryPermissions.Create)]
        public async Task CreateCategoryAsync(DataCategoryCreateInput input)
        {
            var category=await _dataItemManager.CreateCategoryAsync(input.Name, input.Code);
            category.Source =DataItemSource.Input;
            await UpdateCategoryInputAsync(category, input);
            await _repository.InsertAsync(category);
        }

        [Authorize(Policy = DataDictionaryPermissions.Update)]
        public async Task UpdateCategoryAsync(Guid id,DataCategoryUpdate input)
        {
            var category = await _repository.GetAsync(id);
            await _dataItemManager.ChangeCategoryAsync(category,input.Name, input.Code);
            await UpdateCategoryInputAsync(category, input);
            await _repository.UpdateAsync(category);
        }

        public async Task<ListResultDto<DataItemDto>> GetItemsAsync(Guid categoryId)
        {
            return new ListResultDto<DataItemDto>(ObjectMapper.Map<List<DataItem>, List<DataItemDto>>(await _repository.GetListAsync(r => r.ParentId == categoryId)));
        }


        public async Task<ListResultDto<DataItemDto>> GetItemsByParentCodeAsync(string code)
        {
            var category=await _repository.GetAsync(d=>d.Code== code);
            return new ListResultDto<DataItemDto>(ObjectMapper.Map<List<DataItem>, List<DataItemDto>>(await _repository.GetListAsync(r => r.ParentId == category.Id)));
        }

        [Authorize(Policy = DataDictionaryPermissions.Create)]
        public async Task CreateAsync(DataItemCreateInput input)
        {
            await _repository.GetAsync(input.DataCategoryId);
            var item = await _dataItemManager.CreateAsync(input.Name, input.Code,input.Value,input.DataCategoryId);
            item.Source = DataItemSource.Input;
            await UpdateInputAsync(item, input);
            await _repository.InsertAsync(item);
        }

        [Authorize(Policy = DataDictionaryPermissions.Update)]
        public async Task UpdateAsync(Guid id,DataItemCreateUpdate input)
        {
            var item = await _repository.GetAsync(id);

            await _dataItemManager.ChangeItemAsync(item, input.Name, input.Code, input.Value,item.ParentId.Value);
            await UpdateInputAsync(item, input);
            await _repository.UpdateAsync(item);
        }

        [Authorize(Policy = DataDictionaryPermissions.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return _dataItemManager.DeleteAsync(id, false);
        }

        #region 私有方法


        private async Task UpdateInputAsync(DataItem item, DataItemCreateInput input)
        {
            item.SetStatus(input.Status);
            item.Description = input.Description;
            item.Order = input.Order;
        }


        private async Task UpdateCategoryInputAsync(DataItem item, DataCategoryCreateInput input)
        {
            item.SetStatus(input.Status);
            item.Description = input.Description;
            item.Order = input.Order;
        }


        #endregion

    }
}
