using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Visit.Service.BusinessLogic.BlobStorage
{
    public interface IBlobStorageBusinessLogic
    {
        Task<List<string>> ListFiles();
        Task<bool> UploadFile(string fileName, IFormFile asset);
        Task<string> GetFileByName(string fileName);
        Task<bool> DeleteFile(string fileName);
    }
}