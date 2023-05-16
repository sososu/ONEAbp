using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace ONE.Abp.FileManagement.Files
{

    [Area(FileManagementRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
    [Route("api/file-management/file")]
    public class FileController : FileManagementController, IFileAppService
    {

        public readonly IFileAppService _fileAppService;

        public FileController(IFileAppService fileAppService)
        { 
            _fileAppService = fileAppService; 
        }

        [HttpPost]
        public Task<FileUploadOutputDto> CreateAsync([FromForm]FileUploadInputDto input)
        {
            return _fileAppService.CreateAsync(input);
        }

        [HttpDelete("{name}")]
        public Task<bool> DeleteAsync(string name)
        {
           return _fileAppService.DeleteAsync(name);
        }

        [HttpPost("download")]
        public Task<IRemoteStreamContent> DownloadFileAsync(string name)
        {
            return _fileAppService.DownloadFileAsync(name);
        }

        [HttpGet("{name}")]
        public Task<RawFileDto> GetAsync(string name)
        {
           return _fileAppService.GetAsync(name);
        }

        [HttpGet("stream/{name}")]
        public Task<IRemoteStreamContent> GetFileStreamAsync(string name)
        {
            return _fileAppService.GetFileStreamAsync(name);
        }

        [HttpGet("statistics")]
        public Task<FileStatisticsDto> GetStatisticsAsync()
        {
            return _fileAppService.GetStatisticsAsync();
        }

        [HttpGet("page")]
        public Task<PagedResult<FileRecordDto>> QueryPageAsync(FileQueryInput input)
        {
            return _fileAppService.QueryPageAsync(input);
        }
    }
}
