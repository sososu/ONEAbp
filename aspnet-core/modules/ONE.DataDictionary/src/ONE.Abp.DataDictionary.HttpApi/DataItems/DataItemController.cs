using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataDictionary.DataItems
{
    /// <summary>
    /// 字典服务
    /// </summary>

    [Area(DataDictionaryRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = DataDictionaryRemoteServiceConsts.RemoteServiceName)]
    [Route("api/data-dictionary/datas")]
    public class DataItemController : DataDictionaryController, IDataItemAppService
    {
        private readonly IDataItemAppService _itemAppService;

        public DataItemController(IDataItemAppService itemAppService)
        {
            _itemAppService = itemAppService;
        }

        /// <summary>
        /// 创建字典类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("category")]
        public Task CreateCategoryAsync(DataCategoryCreateInput input)
        {
            return _itemAppService.CreateCategoryAsync(input);
        }

        /// <summary>
        /// 更新字典类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("category/{id}")]
        public Task UpdateCategoryAsync(Guid id, DataCategoryUpdate input)
        {
            return _itemAppService.UpdateCategoryAsync(id,input);
        }

        /// <summary>
        /// 获取字典类集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("categorys")]
        public Task<ListResultDto<DataItemDto>> GetCategoryAsync()
        {
            return _itemAppService.GetCategoryAsync();
        }

        /// <summary>
        /// 创建字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPost("item")]
        public Task CreateAsync(DataItemCreateInput input)
        {
            return _itemAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPut("item/{id}")]
        public Task UpdateAsync(Guid id, DataItemCreateUpdate input)
        {
            return _itemAppService.UpdateAsync(id,input);
        }

        /// <summary>
        /// 获取字典项集合
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("items")]
        public Task<ListResultDto<DataItemDto>> GetItemsAsync(Guid categoryId)
        {
            return _itemAppService.GetItemsAsync(categoryId);
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _itemAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取启用的字典类别
        /// </summary>
        /// <returns></returns>
        [HttpGet("enable/categorys")]
        public Task<ListResultDto<DataItemDto>> GetEnableCategoryAsync()
        {
            return _itemAppService.GetEnableCategoryAsync();
        }

        /// <summary>
        /// 获取启用的字典项
        /// </summary>
        /// <returns></returns>
        [HttpGet("enable/items")]
        public Task<ListResultDto<DataItemDto>> GetEnableItemsAsync(Guid categoryId)
        {
            return _itemAppService.GetEnableItemsAsync(categoryId);
        }

        /// <summary>
        /// 根据字典类别编码获取字典项集合
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("items/bycode")]
        public Task<ListResultDto<DataItemDto>> GetItemsByParentCodeAsync(string code)
        {
            return _itemAppService.GetItemsByParentCodeAsync(code);
        }
    }
}
