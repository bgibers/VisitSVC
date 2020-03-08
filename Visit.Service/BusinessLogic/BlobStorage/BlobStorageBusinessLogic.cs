using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Visit.Service.Config;

namespace Visit.Service.BusinessLogic.BlobStorage
{
    public class BlobStorageBusinessLogic : IBlobStorageBusinessLogic
    {
        private readonly IOptions<BlobConfig> _config;
        private readonly ILogger<BlobStorageBusinessLogic> _logger;

        public BlobStorageBusinessLogic(IOptions<BlobConfig> config, ILogger<BlobStorageBusinessLogic> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<List<string>> ListFiles()
        {
            var blobs = new List<string>();
            try
            {
                if (CloudStorageAccount.TryParse(_config.Value.StorageConnection,
                    out var storageAccount))
                {
                    var blobClient = storageAccount.CreateCloudBlobClient();

                    var container = blobClient.GetContainerReference(_config.Value.Container);

                    var resultSegment = await container.ListBlobsSegmentedAsync(null);
                    foreach (var item in resultSegment.Results)
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            var blob = (CloudBlockBlob) item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudPageBlob))
                        {
                            var blob = (CloudPageBlob) item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            var dir = (CloudBlobDirectory) item;
                            blobs.Add(dir.Uri.ToString());
                        }
                }

                _logger.LogCritical("Can't read Blob Storage connection from config");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return blobs;
        }

        public async Task<bool> UploadFile(string fileName, IFormFile asset)
        {
            try
            {
                if (CloudStorageAccount.TryParse(_config.Value.StorageConnection,
                    out var storageAccount))
                {
                    var blobClient = storageAccount.CreateCloudBlobClient();

                    var container = blobClient.GetContainerReference(_config.Value.Container);

                    var blockBlob = container.GetBlockBlobReference(fileName);

                    await blockBlob.UploadFromStreamAsync(asset.OpenReadStream());

                    return true;
                }

                _logger.LogCritical("Can't read Blob Storage connection from config");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        public async Task<string> GetFileByName(string fileName)
        {
            try
            {
                var ms = new MemoryStream();

                if (CloudStorageAccount.TryParse(_config.Value.StorageConnection,
                    out var storageAccount))
                {
                    var blobClient = storageAccount.CreateCloudBlobClient();
                    var container = blobClient.GetContainerReference(_config.Value.Container);
                    if (await container.ExistsAsync())
                    {
                        var file = container.GetBlobReference(fileName);
                        await file.FetchAttributesAsync();
                        var arr = new byte[file.Properties.Length];
                        await file.DownloadToByteArrayAsync(arr, 0);
                        var fileBase64 = Convert.ToBase64String(arr);
                        return fileBase64;
                    }

                    _logger.LogCritical(new StorageException("Container does not exist").ToString());
                }

                _logger.LogCritical("Can't read Blob Storage connection from config");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return null;
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                if (CloudStorageAccount.TryParse(_config.Value.StorageConnection,
                    out var storageAccount))
                {
                    var blobClient = storageAccount.CreateCloudBlobClient();
                    var container = blobClient.GetContainerReference(_config.Value.Container);

                    if (await container.ExistsAsync())
                    {
                        var file = container.GetBlobReference(fileName);

                        if (await file.ExistsAsync()) await file.DeleteAsync();
                    }
                    else
                    {
                        _logger.LogCritical(new StorageException("Container does not exist").ToString());
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}