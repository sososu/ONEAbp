using AutoMapper;
using ONE.Abp.FileManagement.FileRecords;
using ONE.Abp.FileManagement.Files;

namespace ONE.Abp.FileManagement;

public class FileManagementApplicationAutoMapperProfile : Profile
{
    public FileManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<FileRecord, FileRecordDto>();
    }
}
