using BaharShop.Domain.UploadFile;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BaharShop.Persistence.Repository.UploadFile
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string _storagePath;

        public FileUploadService(IConfiguration configuration)
        {
            _storagePath = configuration["FileUpload:StoragePath"] ?? string.Empty;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or not provided.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(_storagePath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public string GetStoragePath()
        {
            return _storagePath;
        }
    }
}
