using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Visit.Service.Config;

namespace Visit.Service.BusinessLogic.BlobStorage
{
    public class BlobStorageBusinessLogic : IBlobStorageBusinessLogic
    {
        private readonly ILogger<BlobStorageBusinessLogic> _logger;
        private readonly BlobContainerClient _storageContainer;
        private readonly string _accountName;

        public BlobStorageBusinessLogic(IOptions<BlobConfig> config, ILogger<BlobStorageBusinessLogic> logger)
        {
            if (string.IsNullOrEmpty(config.Value.StorageConnection))
            {
                throw new Exception("Storage connection is null");
            }

            if (string.IsNullOrEmpty(config.Value.Container))
            {
                throw new Exception("Container name is null");
            }

            _logger = logger;
            _storageContainer = new BlobContainerClient(config.Value.StorageConnection, config.Value.Container);
            _storageContainer.CreateIfNotExists();
            _storageContainer.SetAccessPolicy(PublicAccessType.Blob);

            _accountName = _storageContainer.AccountName;
        }

        /// <inheritdoc />
        public string GetAccountName()
        {
            return _accountName;
        }

        /// <inheritdoc />
        public async Task<bool> VerifyContainersExistence()
        {
            return await _storageContainer.ExistsAsync();
        }

        /// <inheritdoc />
        public async Task<bool> CheckBlobExistence(string blobName)
        {
            var blob = GetBlob(blobName);
            var exists = (await blob.ExistsAsync()).Value;

            _logger.LogInformation($"Blob at {blobName} existence:{exists}");
            return exists;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteBlobIfExists(string blobName)
        {
            return await _storageContainer.DeleteBlobIfExistsAsync(blobName);
        }

        /// <inheritdoc />
        public async Task<Uri> UploadBlob(string blobPath, IFormFile file, Guid fileName)
        {
            try
            {
                var blob = GetBlob($"{blobPath}/{fileName}.jpg");
                BlobHttpHeaders httpHeaders = new BlobHttpHeaders
                { 
                    ContentType = "image/jpeg"
                };
                using (var stream = file.OpenReadStream())
                {
                   await blob.UploadAsync(stream, httpHeaders);
                   _logger.LogInformation($"Blob at {blobPath} uploaded successfully");
                   return blob.Uri;
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Blob: {JsonConvert.SerializeObject(file)} at {blobPath} could not upload: {e}");
                return new Uri("");
            }
        }

        /// <inheritdoc />
        public async Task<string> GetBlobContents(string blobName)
        {
            var blob = GetBlob(blobName);
            return await ReadBlobAsync(blob);
        }

        /// <inheritdoc />
        public BlobClient GetBlob(string blobPath)
        {
            return _storageContainer.GetBlobClient(blobPath);
        }

        /// <inheritdoc />
        public IEnumerable<string> ListDirectoryContents(string directory)
        {
            // once we're on 3.1 we can move to
            // await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            var blobs = GetBlobFiles(directory);
            return blobs.Select(b => b.Name);
        }

        /// <inheritdoc />
        public IEnumerable<BlobItem> GetBlobFiles(string directory)
        {
            return _storageContainer.GetBlobs(BlobTraits.Metadata, BlobStates.None, prefix: directory);
        }

        /// <inheritdoc />
        public async Task UpdateBlobMetadata(string blobName, IDictionary<string, string> metadata)
        {
            await _storageContainer.GetBlobClient(blobName).SetMetadataAsync(metadata);
        }


        private async Task<string> ReadBlobAsync(BlobClient blob)
        {
            if ((await blob.ExistsAsync()).Value)
            {

                using (var stream = (await blob.DownloadAsync()).Value.Content)
                using (var streamReader = new StreamReader(stream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }

            throw new Exception($"Unable to find blob for {blob.Name}");
        }
    }
}