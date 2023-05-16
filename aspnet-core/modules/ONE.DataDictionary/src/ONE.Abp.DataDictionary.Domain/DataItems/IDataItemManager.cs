using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace ONE.Abp.DataDictionary.DataItems
{
    public interface IDataItemManager
    {
        [NotNull]
        Task<DataItem> CreateAsync([NotNull] string name, [NotNull] string code,[NotNull] int value, [NotNull] Guid parentId);

        Task ChangeItemAsync([NotNull] DataItem dataItem, [NotNull] string name, [NotNull] string code, [NotNull] int value, [NotNull] Guid parentId);


        [NotNull]
        Task<DataItem> CreateCategoryAsync([NotNull] string name, [NotNull] string code);

        Task ChangeCategoryAsync([NotNull] DataItem dataItem, [NotNull] string name, [NotNull] string code);

        Task DeleteAsync(Guid id, bool canDelEnum);
    }
}
