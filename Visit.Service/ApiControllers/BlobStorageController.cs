using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visit.Service.BusinessLogic.BlobStorage;

namespace Visit.Service.ApiControllers
{
    [Route("storage")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageBusinessLogic _blobStorageBusinessLogic;

        public BlobStorageController(IBlobStorageBusinessLogic blobStorageBusinessLogic)
        {
            _blobStorageBusinessLogic = blobStorageBusinessLogic;
        }

        [HttpGet("ListFiles/{userId}")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public IEnumerable<string> ListFiles(string userId)
        {
            return _blobStorageBusinessLogic.ListDirectoryContents(userId);
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(string fileName, IFormFile asset)
        {
            return Ok(await _blobStorageBusinessLogic.UploadBlob(fileName, asset));
        }

        [HttpGet("DownloadFile/{fileName}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<string> DownloadFile(string fileName)
        {
            return await _blobStorageBusinessLogic.GetBlobContents(fileName);
        }


        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            return Ok(await _blobStorageBusinessLogic.DeleteBlobIfExists(fileName));
        }
    }
}