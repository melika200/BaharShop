using BaharShop.Domain.UploadFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpGet("GetFile")]
        public IActionResult GetFile(string filename)
        {
            var filePath = Path.Combine(_fileUploadService.GetStoragePath(), filename);
            if (System.IO.File.Exists(filePath))
            {
                return File(System.IO.File.OpenRead(filePath), "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
