using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BaharShop.Domain.UploadFile
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(string filePath);
        string GetStoragePath();
    }
}
