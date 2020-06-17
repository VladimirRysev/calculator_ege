using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string directoryName);

        Task RemoveFile(string filePath);
    }
}
