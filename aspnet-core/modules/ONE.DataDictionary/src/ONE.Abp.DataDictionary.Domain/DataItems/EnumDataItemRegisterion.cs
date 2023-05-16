using ONE.Abp.Data.DataDics;

namespace ONE.Abp.DataDictionary.DataItems
{
    public class ShelfEnumDataItemRegisterion : EnumDataItemRegisterionBase
    {
        public override void Register(AbpEnumDicOption option)
        {
            option.Add<DataItemStatus>("字典状态");
            option.Add<DataItemSource>("字典来源");
        }
    }
}
