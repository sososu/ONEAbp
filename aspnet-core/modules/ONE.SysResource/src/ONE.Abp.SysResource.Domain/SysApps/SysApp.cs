using JetBrains.Annotations;
using System;
using System.Reflection.Emit;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace ONE.Abp.SysResource.SysApps
{
    /// <summary>
    /// 系统应用
    /// </summary>
    public class SysApp : AuditedAggregateRoot<Guid>
    {
        public string AppName { get; protected set; }

        public string AppVersion { get; protected set; }

        public string AppCode { get; protected set; }

        [CanBeNull]
        public string AppSecret { get; protected set; }

        [CanBeNull]
        public string AppUrl { get; protected set; }

        [CanBeNull]
        public string Description { get; protected set; }

        protected SysApp()
        {

        }


        public SysApp(Guid id,string code)
        {
            Check.NotNull(id, nameof(id));
            Id = id;

            SetAppCode(code);
        }

        public void SetBasicInfo(string appName, string appVersion)
        {
            Check.NotNull(appName, nameof(appName));
            Check.NotNull(appVersion, nameof(appVersion));

            AppName = appName;
            AppVersion = appVersion;
        }

        public void SetDescription(string description)
        {
            Description= description;
        }

        public void SetUrl(string url)
        {
            AppUrl = url;
        }

        public void SetSecret(string secret)
        {
            AppSecret = secret;
        }

        public void SetAppCode(string appCode)
        {
            Check.NotNull(appCode, nameof(appCode));
            AppCode = appCode;
        }
    }
}
