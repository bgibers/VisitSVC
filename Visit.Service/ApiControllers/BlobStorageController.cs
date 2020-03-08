using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visit.Service.BusinessLogic.BlobStorage;

namespace Visit.Service.ApiControllers
{
    // TODO change method of getting file?


    [Route("storage")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageBusinessLogic _blobStorageBusinessLogic;

        public BlobStorageController(IBlobStorageBusinessLogic blobStorageBusinessLogic)
        {
            _blobStorageBusinessLogic = blobStorageBusinessLogic;
        }

        [HttpGet("ListFiles")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<List<string>> ListFiles()
        {
            return await _blobStorageBusinessLogic.ListFiles();
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(string fileName, IFormFile asset)
        {
            return Ok(await _blobStorageBusinessLogic.UploadFile(fileName, asset));
        }

        [HttpGet("DownloadFile/{fileName}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<string> DownloadFile(string fileName)
        {
            return await _blobStorageBusinessLogic.GetFileByName(fileName);
        }


        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            return Ok(await _blobStorageBusinessLogic.DeleteFile(fileName));
        }
    }
}