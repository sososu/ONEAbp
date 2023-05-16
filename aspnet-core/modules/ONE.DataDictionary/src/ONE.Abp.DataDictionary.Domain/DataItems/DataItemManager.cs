using JetBrains.Annotations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ONE.Abp.DataDictionary.DataItems
{
    public class DataItemManager : DomainService, IDataItemManager
    {
        protected IRepository<DataItem, Guid> Repository;

        public DataItemManager(IRepository<DataItem, Guid> repository)
        {
            Repository = repository;

        }

        public async Task<DataItem> CreateAsync([NotNull] string name, [NotNull] string code,[NotNull] int value, [NotNull] Guid parentId)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(code, nameof(code));
            Check.NotNull(value, nameof(value));
            Check.NotNull(parentId, nameof(parentId));

            await ValidateCodeAsync(code,parentId);
            await ValidateValueAsync(value,parentId);
            await ValidateNameAsync(name,parentId);
            return new DataItem(GuidGenerator.Create(),name,code,value,parentId);
        }

        public async Task ChangeItemAsync([NotNull] DataItem dataItem, [NotNull] string name, [NotNull] string code, [NotNull] int value, [NotNull] Guid parentId)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(code, nameof(code));
            Check.NotNull(value, nameof(value));
            Check.NotNull(parentId, nameof(parentId));

            if(dataItem.Source==DataItemSource.Enum)
            {
                if(dataItem.Code!=code) { throw new BusinessException(DataDictionaryErrorCodes.FromEnumSourceCannotEditCode); }

                if(dataItem.Value!=value) { throw new BusinessException(DataDictionaryErrorCodes.FromEnumSourceCannotEditValue); }
            }

            await ValidateCodeAsync(code, parentId,dataItem.Id);
            await ValidateValueAsync(value, parentId, dataItem.Id);
            await ValidateNameAsync(name, parentId, dataItem.Id);
            dataItem.SetName(name);
            dataItem.SetCode(code);
            dataItem.SetValue(value);
            dataItem.SetParentId(parentId);
        }

        public async Task<DataItem> CreateCategoryAsync([NotNull] string name, [NotNull] string code)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(code, nameof(code));

            await ValidateNameAsync(name);
            await ValidateCodeAsync(code);
            return new DataItem(GuidGenerator.Create(), name, code);
        }

        public async Task ChangeCategoryAsync([NotNull] DataItem dataItem, [NotNull] string name, [NotNull] string code)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(code, nameof(code));

            if (dataItem.Source == DataItemSource.Enum)
            {
                if (dataItem.Code != code) { throw new BusinessException(DataDictionaryErrorCodes.FromEnumSourceCannotEditCode); }
            }

            await ValidateNameAsync(name,null,dataItem.Id);
            await ValidateCodeAsync(code, null, dataItem.Id);
            dataItem.SetName(name);
            dataItem.SetCode(code);
        }

        public async Task DeleteAsync(Guid id,bool canDelEnum)
        {
            var item = await Repository.GetAsync(id);
            if (!canDelEnum&&item.Source == DataItemSource.Enum)
                throw new BusinessException(DataDictionaryErrorCodes.FromEnumSourceCannotDelete);

            if (!item.ParentId.HasValue)
                await Repository.DeleteAsync(r => r.ParentId == id);

            await Repository.DeleteAsync(id);
        }

        protected virtual async Task ValidateValueAsync(long value, Guid parentId, Guid? expectedId = null)
        {
            var item = await Repository.FindAsync(r => r.ParentId == parentId && r.Value == value);
            if (item != null && item.Id != expectedId)
            {
                throw new BusinessException(DataDictionaryErrorCodes.DuplicateDataItemValue).WithData("Value", value);
            }
        }


        protected virtual async Task ValidateCodeAsync(string code, Guid? parentId=null, Guid? expectedId = null)
        {
            Expression<Func<DataItem, bool>> filter = q => q.Code == code;
            if (parentId.HasValue)
                filter.And(q => q.ParentId == parentId.Value);
            else
                filter.And(q => !q.ParentId.HasValue);

            var item = await Repository.FindAsync(filter);

            if (item != null && item.Id != expectedId)
            {
                throw new BusinessException(DataDictionaryErrorCodes.DuplicateDataItemCode).WithData("Code", code);
            }
        }


        protected virtual async Task ValidateNameAsync(string name, Guid? parentId=null, Guid? expectedId = null)
        {
            Expression<Func<DataItem, bool>> filter = q => q.Name == name;
            if (parentId.HasValue)
                filter.And(q => q.ParentId == parentId.Value);
            else
                filter.And(q => !q.ParentId.HasValue);

            var item = await Repository.FindAsync(filter);

            if (item != null && item.Id != expectedId)
            {
                throw new BusinessException(DataDictionaryErrorCodes.DuplicateDataItemName).WithData("Name", name);
            }
        }

    }
}
