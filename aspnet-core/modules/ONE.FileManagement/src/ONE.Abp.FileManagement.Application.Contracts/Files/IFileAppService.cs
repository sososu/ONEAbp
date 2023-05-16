using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace ONE.Abp.FileManagement.Files
{
    public interface IFileAppService : IApplicationService
    {

        public Task<PagedResult<FileRecordDto>> QueryPageAsync(FileQueryInput input);

        public Task<FileStatisticsDto> GetStatisticsAsync();
        public Task<RawFileDto> GetAsync(string name);

        public Task<IRemoteStreamContent> GetFileStreamAsync(string name);

        public Task<bool> DeleteAsync(string name);

        public Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input);

        public Task<IRemoteStreamContent> DownloadFileAsync(string name);
    }
}
