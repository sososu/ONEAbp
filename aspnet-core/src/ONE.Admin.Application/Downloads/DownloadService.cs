using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ONE.Admin.Downloads
{
    public class DownloadService : AdminAppService, IDownloadService
    {
        private DownloadOption _downloadOption;

        public DownloadService(IOptions<DownloadOption> options)
        {
            _downloadOption = options.Value;
        }

        /*type=template  name=template name*/

        public async Task<GetVersionResultDto> GetVersionAsync(string type, GetLatestSourceCodeVersionDto input)
        {
            var lastVersion = _downloadOption.DownloadVersions.Where(d => d.Type == type && d.Name == input.Name)
                .WhereIf(!input.IncludePreReleases, d => !d.IsPrerelease)
                .OrderByDescending(d => d.Version).FirstOrDefault()?.Version ?? "1.1.0";

            return new GetVersionResultDto { Version = lastVersion };
        }



        public async Task<GithubReleaseVersions> GetAllVersionAsync(bool includePreReleases)
        {
            var fversions = _downloadOption.DownloadVersions.WhereIf(!includePreReleases, d => !d.IsPrerelease).Select(d => new GithubRelease
            {
                Id = d.Id,
                Name = d.Version,
                IsPrerelease = d.IsPrerelease,
                PublishTime = DateTime.Now,
            }).ToList();

            return new GithubReleaseVersions
            {
                FrameworkAndCommercialVersions = fversions
            };
        }


        public async Task<GetVersionResultDto> GetNugetVersionAsync(string type, GetTemplateNugetVersionDto input)
        {
            var lastVersion = _downloadOption.DownloadVersions.Where(d => d.Type == type && d.Name == input.Name && d.Version == input.Version)
               .WhereIf(!input.IncludePreReleases, d => !d.IsPrerelease)
               .OrderByDescending(d => d.Version).FirstOrDefault()?.Version ?? "1.1.0";

            return new GetVersionResultDto { Version = lastVersion };
        }

     
        public async Task<byte[]> DownloadAsync(string type, SourceCodeDownloadInputDto input)
        {
            var version = _downloadOption.DownloadVersions.Where(d => d.Type == type && d.Name == input.Name && d.Version == input.Version)
               .WhereIf(!input.IncludePreReleases, d => !d.IsPrerelease)
               .FirstOrDefault();

            if(version == null) { return null; }


            var filePath = Path.Combine(_downloadOption.FileBaseFolderPath,input.Name, $"{input.Name}-{input.Version}.zip");

            return File.ReadAllBytes(filePath);
        }

    }
}
