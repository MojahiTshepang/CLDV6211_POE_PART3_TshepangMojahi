using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace LFPEvents.Helpers
{
    public class BlobHelper
    {
        private static string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=lfpstorage123;AccountKey=6qluEoFoKHnd1hjfr3lZ+Rp2Sbg4jKNBn89LFjhZGZarOao6P5pIHYbSI+DUc2d9rullsssJRKDZ+AStqga0qA==;EndpointSuffix=core.windows.net";
        private static string containerName = "venueimages";

        public static async Task<string> UploadFileAsync(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(uniqueFileName);

            using (var stream = file.InputStream)
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }

            return blockBlob.Uri.ToString(); // 👈 Public URL
        }
    }
}
