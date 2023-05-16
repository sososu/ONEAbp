using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ONE.Admin.Downloads
{
    public class GetLatestSourceCodeVersionDto
    {
        public string Name { get; set; }

        public bool IncludePreReleases { get; set; }
    }

    public class GetVersionResultDto
    {
        public string Version { get; set; }
    }
    public class GetTemplateNugetVersionDto
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public bool IncludePreReleases { get; set; }
    }


    public class GithubReleaseVersions
    {
        public List<GithubRelease> FrameworkAndCommercialVersions { get; set; }

        public List<GithubRelease> LeptonXVersions { get; set; }
    }

    [Serializable]
    [JsonObject]
    public class GithubRelease
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("prerelease")]
        public bool IsPrerelease { get; set; }

        [JsonProperty("published_at")]
        public DateTime PublishTime { get; set; }
    }


    public class SourceCodeDownloadInputDto
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Type { get; set; }

        public string TemplateSource { get; set; }

        public bool IncludePreReleases { get; set; }
    }




    public class DownloadOption
    {
        public string FileBaseFolderPath { get; set; }

        public List<DownloadVersion> DownloadVersions { get; set;}
    }

    public class DownloadVersion
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Version { get; set; }

        public string NugetVersion { get; set; }

        public string Type { get; set; }

        public bool IsPrerelease { get; set; }
    }
}
