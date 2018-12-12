using JukeBox.Mobile.Common;
using JukeBox.Mobile.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Mobile.Services
{
    public class AzureService : IAzureService
    {
        private readonly CloudBlobContainer _container;

        public AzureService()
        {
            var storageAccount = CloudStorageAccount.Parse(Constants.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(Constants.StorageContainerName);
        }

        public async Task<IEnumerable<MediaFile>> GetAllFilesAsync()
        {
            BlobContinuationToken continuationToken = null;
            var blobOptions = new BlobRequestOptions();
            var allFilesUri = new List<MediaFile>();

            do
            {
                var listingResult = await _container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = listingResult.ContinuationToken;
                allFilesUri.AddRange(listingResult.Results.
                    Select(s => new MediaFile { FileUri = s.Uri,
                        FileName = ((CloudBlob)s).Name }));
            } while (continuationToken != null);

            return allFilesUri;
        }
    }
}
