﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _storagePath;

        public FileController(IConfiguration configuration)
        {
            _storagePath = configuration["FileUpload:StoragePath"];

        }

        [HttpGet("GetFile")]
        public IActionResult DownloadFile(string fileName)
        {
            var fullPath = Path.Combine(_storagePath, fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var contentType = "application/octet-stream"; // Change this based on the file type

            return PhysicalFile(fullPath, contentType, fileName);
        }
    }
}
