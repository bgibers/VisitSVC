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
            List<string> blobs = new List<string>();
            try
            {
                if (CloudStorageAccount.TryParse(_config.Value.StorageConnection,
                    out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(_config.Value.Container);

                    BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(null);
                    foreach (IListBlobItem item in resultSegment.Results)
                    {
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob = (CloudBlockBlob) item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudPageBlob))
                        {
                            CloudPageBlob blob = (CloudPageBlob) item;
                            blobs.Add(blob.Name);
                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            CloudBlobDirectory dir = (CloudBlobDirectory) item;
                            blobs.Add(dir.Uri.ToString());
                        }
                    }
                }
                _logger.LogCritical("Can't read Blob Storage connection from config");
            }
            catch(Exception e)
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
                    out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(_config.Value.Container);

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
    
                    await blockBlob.UploadFromStreamAsync(asset.OpenReadStream());

                    return true;
                }
                _logger.LogCritical("Can't read Blob Storage connection from config");
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            
            return false;
        }

        public async Task<string> GetFileByName(string fileName)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                
                if (CloudStorageAccount.TryParse(_config.Value.StorageConnection,
                    out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(_config.Value.Container);
                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(fileName);
                        await file.FetchAttributesAsync();
                        byte[] arr = new byte[file.Properties.Length];
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
                    out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(_config.Value.Container);

                    if (await container.ExistsAsync())
                    {
                        CloudBlob file = container.GetBlobReference(fileName);

                        if (await file.ExistsAsync())
                        {
                            await file.DeleteAsync();
                        }
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