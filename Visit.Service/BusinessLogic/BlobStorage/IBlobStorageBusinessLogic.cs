using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Visit.Service.BusinessLogic.BlobStorage
{
    public interface IBlobStorageBusinessLogic
    {
        string GetAccountName();
        Task<bool> VerifyContainersExistence();
        Task<bool> CheckBlobExistence(string blobName);
        Task<bool> DeleteBlobIfExists(string blobName);
        Task<bool> UploadBlob(string blobPath, IFormFile body);
        Task<string> GetBlobContents(string blobName);
        BlobClient GetBlob(string blobPath);
        IEnumerable<string> ListDirectoryContents(string directory);
        IEnumerable<BlobItem> GetBlobFiles(string directory);
        Task UpdateBlobMetadata(string blobName, IDictionary<string, string> metadata);
    }
}