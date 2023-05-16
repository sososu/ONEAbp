using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Admin.Downloads
{
    public interface IDownloadService : IApplicationService
    {
        /// <summary>
        /// 获取最新版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<GetVersionResultDto> GetVersionAsync(string type, GetLatestSourceCodeVersionDto input);


        /// <summary>
        /// 获取最新版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<GithubReleaseVersions> GetAllVersionAsync(bool includePreReleases = true);


        public Task<GetVersionResultDto> GetNugetVersionAsync(string type, GetTemplateNugetVersionDto input);

        public Task<byte[]> DownloadAsync(string type, SourceCodeDownloadInputDto input);
    }
}
