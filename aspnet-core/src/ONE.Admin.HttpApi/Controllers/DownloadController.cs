using Microsoft.AspNetCore.Mvc;
using ONE.Admin.Downloads;
using System.Threading.Tasks;

namespace ONE.Admin.Controllers
{
    /// <summary>
    /// ONE服务
    /// </summary>
    [Route("api/download")]
    [ApiController]
    public class DownloadController : AdminController
    {
        private readonly IDownloadService _downloadService;

        public DownloadController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="type"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{type}")]
        public async Task<FileContentResult> DownloadAsync(string type, SourceCodeDownloadInputDto input)
        {
            var bytes = await _downloadService.DownloadAsync(type, input);
            return new FileContentResult(bytes, "application/zip")
            {
                FileDownloadName="file.zip"
            };
        }

        /// <summary>
        /// 获取所有版本
        /// </summary>
        /// <param name="includePreReleases"></param>
        /// <returns></returns>
        [HttpGet("all-versions")]

        public Task<GithubReleaseVersions> GetAllVersionAsync(bool includePreReleases)
        {
            return _downloadService.GetAllVersionAsync(includePreReleases);
        }

        /// <summary>
        /// 获取nuget包最新版本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{type}/get-nuget-version")]
        public Task<GetVersionResultDto> GetNugetVersionAsync(string type, GetTemplateNugetVersionDto input)
        {
            return _downloadService.GetNugetVersionAsync(type, input);
        }


        /// <summary>
        /// 获取最新版本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{type}/get-version")]
        public Task<GetVersionResultDto> GetVersionAsync(string type, GetLatestSourceCodeVersionDto input)
        {
            return _downloadService.GetVersionAsync(type, input);
        }
    }
}
