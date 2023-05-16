using AutoMapper;
using ONE.Abp.DataDictionary.DataItems;

namespace ONE.Abp.DataDictionary;

public class DataDictionaryApplicationAutoMapperProfile : Profile
{
    public DataDictionaryApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<DataItem, DataItemDto>();
    }
}
