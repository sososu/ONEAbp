using Microsoft.AspNetCore.Authorization;
using ONE.Abp.Pagination;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.FileManagement.FileRecords;
using ONE.Abp.FileManagement.Permissions;
using ONE.Abp.FileManagement.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.FileManagement.Files
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class FileAppService : FileManagementAppService, IFileAppService
    {
        protected IBlobContainer<OneFileContainer> BlobContainer { get; }

        protected IRepository<FileRecord> Repository { get; }

        const string DEFAULTTAG = "Default";

        private readonly Random random= new Random();
        public FileAppService(
            IBlobContainer<OneFileContainer> blobContainer, IRepository<FileRecord> repository)
        {
            BlobContainer = blobContainer;
            Repository = repository;
        }


        #region 文件管理

        [Authorize(Policy = FileManagementPermissions.Default)]
        public async Task<PagedResult<FileRecordDto>> QueryPageAsync(FileQueryInput input)
        {
            return await (await Repository.GetQueryableAsync()).ToPagedResultAsync<FileRecord, FileRecordDto>(input, "CreationTime DESC");
        }
      
        
        [Authorize(Policy = FileManagementPermissions.Default)]
        public async Task<FileStatisticsDto> GetStatisticsAsync()
        {
            var statistics = new FileStatisticsDto();

            var useTotalSize = await Repository.SumAsync(r => r.FileSize);
            var totalLimitSizeStr = await SettingProvider.GetOrNullAsync(FileManagementSettings.TotalLimitSizeSettingName);
            _ = long.TryParse(totalLimitSizeStr, out var totalLimitSize);
        
            statistics.TotalSize = totalLimitSize;
            statistics.UseSize= useTotalSize;

            statistics.FileTypeDetailStatistics = new List<FileTypeDetailStatistics>();

            foreach (var item in FileTypeHelper.FileTypes)
            {
                var detail = new FileTypeDetailStatistics();
                detail.FileType = item;
                detail.TotalCount= await Repository.CountAsync(r => r.FileType == item);
                statistics.FileTypeDetailStatistics.Add(detail);
            }

            return statistics;
        }

        #endregion

        /// <summary>
        /// 获取文件（字节数组）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Policy = FileManagementPermissions.Default)]
        public virtual async Task<RawFileDto> GetAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return new RawFileDto
            {
                Bytes = await BlobContainer.GetAllBytesAsync(name)
            };
        }

        /// <summary>
        /// 获取文件（文件流）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Policy = FileManagementPermissions.Default)]
        public virtual async Task<IRemoteStreamContent> GetFileStreamAsync(string name)
        {
            var fileStream = await BlobContainer.GetAsync(name);
            return new RemoteStreamContent(fileStream, name, Path.GetExtension(name).GetMIMEByExt(), disposeStream: true);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [Authorize(Policy = FileManagementPermissions.Create)]
        public virtual async Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input)
        {
            if (input.File == null)
            {
                throw new BusinessException(FileManagementErrorCodes.FileCannotBeNull).WithData("Name", nameof(input.File));
            }

            var limitSizeStr = await SettingProvider.GetOrNullAsync(FileManagementSettings.LimitSizeSettingName);
            _ = long.TryParse(limitSizeStr, out var limitSize);
            limitSize = limitSize <= 0 ? SchemeFileWebConsts.FileUploading.MaxFileSize : limitSize;

            if (input.File.ContentLength > limitSize)
            {
                throw new BusinessException(FileManagementErrorCodes.PassFileMaxLimitSize).WithData("Size",Convert.ToInt32((limitSize / 1024f) / 1024f));
            }

            var totalSize =await Repository.SumAsync(r => r.FileSize);
            totalSize += input.File.ContentLength??0;

            var totalLimitSizeStr = await SettingProvider.GetOrNullAsync(FileManagementSettings.TotalLimitSizeSettingName);
            _ = long.TryParse(totalLimitSizeStr, out var totalLimitSize);
            totalLimitSize = totalLimitSize <= 0 ? SchemeFileWebConsts.FileUploading.TotalMaxFileSize : totalLimitSize;

            if (totalSize > totalLimitSize)
            {
                throw new BusinessException(FileManagementErrorCodes.PassFileTotalMaxLimitSize).WithData("Size", Convert.ToInt32((totalLimitSize / 1024f) / 1024f));
            }


            var position = input.File.GetStream().Position;


            var mine = input.File.ContentType;
            var supportMimeType = await SettingProvider.GetOrNullAsync(FileManagementSettings.SupportMimeType);
            var supportMineTypes = supportMimeType.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (!supportMineTypes.Contains(mine))
            {
                throw new BusinessException(FileManagementErrorCodes.FileFormatNotSupport).WithData("Mine", mine);
            }

            //获得到文件名
            string fileName = Path.GetFileName(input.File.FileName.ToString());
            //获得文件扩展名
            string fileNameEx = Path.GetExtension(fileName);

            if (fileNameEx.IsNullOrWhiteSpace())
                fileNameEx = mine.GetDefaultExtByMIME();
           

            //string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var uniqueFileName = GenerateUniqueFileName(fileNameEx,null,random.Next(1,10000).ToString("D4"));

            // IsValidImage may change the position of the stream
            if (input.File.GetStream().CanSeek)
            {
                input.File.GetStream().Position = position;
            }

            await BlobContainer.SaveAsync(uniqueFileName, input.File.GetStream());

            var file = new FileRecord(GuidGenerator.Create(), null, fileName, uniqueFileName, mine.GetFileType(), mine,input.File.ContentLength ?? 0);
            file.Tag = input.Tag ?? DEFAULTTAG;
            await Repository.InsertAsync(file);

            return new FileUploadOutputDto
            {
                DisplayName = fileName,
                Name = uniqueFileName,
                WebUrl= $"/api/file-management/file/stream/{uniqueFileName}"
            };
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Policy = FileManagementPermissions.Delete)]
        public async Task<bool> DeleteAsync(string name)
        {
            var bl = await BlobContainer.DeleteAsync(name);
            if (bl)
                await Repository.DeleteAsync(f => f.FileName == name);
            return bl;
        }


        /// <summary>
        /// 获取文件（文件流）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Policy = FileManagementPermissions.Download)]
        public virtual async Task<IRemoteStreamContent> DownloadFileAsync(string name)
        {
            var fileStream = await BlobContainer.GetAsync(name);
            return new RemoteStreamContent(fileStream, name, Path.GetExtension(name).GetMIMEByExt(), disposeStream: true);
        }

        #region 私有方法
        //private static string GetByExtension(string extension)
        //{
        //    extension = extension.RemovePreFix(".").ToLowerInvariant();

        //    switch (extension)
        //    {
        //        case "png":
        //            return "image/png";
        //        case "gif":
        //            return "image/gif";
        //        case "jpg":
        //        case "jpeg":
        //            return "image/jpeg";

        //        //TODO: Add other extensions too..

        //        default:
        //            return "application/octet-stream";
        //    }
        //}

        private string GenerateUniqueFileName(string extension, string prefix = null, string postfix = null)
        {
            return prefix + GuidGenerator.Create().ToString("N") + postfix + extension;
        }

        #endregion
    }
}
