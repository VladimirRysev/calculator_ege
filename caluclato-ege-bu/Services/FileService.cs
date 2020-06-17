using Calculator_ege_bu.Constants;
using DataAccessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Services
{
    public class FileService : IFileService
    {
        private static readonly HashSet<String> AllowedImageExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif", ".svg" };

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment hostingEnvironment,
            ILogger<FileService> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        public async Task RemoveFile(string filePath)
        {
            try
            {
                filePath = filePath.Replace('/', '\\');
                var fileImagePath = _hostingEnvironment.WebRootPath + filePath;
                File.Delete(fileImagePath);
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete image.\n{e}");
                throw new Exception($"Add image.\n{e}");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string directoryName)
        {
            if (file == null)
            {
                _logger.LogWarning("Add not-existing file");
                throw new Exception("Add not-existing file");
            }

            var userFileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(userFileName).ToLower();

            if (!CheckImageType(fileExt))
            {
                _logger.LogWarning($"File with invalid extension {fileExt}");
                throw new Exception("File with invalid extension");
            }

            try
            {

                var fileName = directoryName + $@"{Guid.NewGuid()}";

                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, directoryName, fileName + fileExt);

                using (var fileStream =
                    new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await file.CopyToAsync(fileStream);
                }
                
                 return fileName + fileExt;
            }
            catch (Exception e)
            {
                _logger.LogError($"Add image.\n{e}");
                throw new Exception($"Add image.\n{e}");
            }
            
        }
        public bool CheckImageType(String fileExt)
        {
            return AllowedImageExtensions.Contains(fileExt);
        }

    }
}
