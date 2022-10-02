using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? 
                                throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            // The-Little-Book-on-REST-Services.pdf
            // demo code
            var filePath = "The-Little-Book-on-REST-Services.pdf";

            // check whether the file exists
            if (!System.IO.File.Exists(filePath)) return NotFound();

            if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                // if it can't be determined, we set it to application/octet-stream
                // which is a default media type for arbitrary binary data.
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }
        
    }
}