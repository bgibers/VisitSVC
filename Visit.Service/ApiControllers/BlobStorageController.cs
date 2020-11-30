using System;
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
        public async Task<ActionResult<Uri>> UploadFile(string filePath, IFormFile asset)
        {
            return await _blobStorageBusinessLogic.UploadBlob(filePath, asset, Guid.NewGuid());
        }

        [HttpGet("DownloadFile/{fileName}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> DownloadFile(string fileName)
        {
            return await _blobStorageBusinessLogic.GetBlobContents(fileName);
        }


        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<ActionResult<bool>> DeleteFile(string fileName)
        {
            return await _blobStorageBusinessLogic.DeleteBlobIfExists(fileName);
        }
    }
}