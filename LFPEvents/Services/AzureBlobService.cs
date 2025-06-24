using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LFPEvents.Services
{
    public class AzureBlobService
    {
        private readonly string _connectionString = "<your_connection_string>";
        private readonly string _containerName = "venue-images"; // or event-images, etc.

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                // Create blob client
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                // Create the container if it does not exist
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                // Upload the file
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(fileStream, overwrite: true);

                // Return the URL of the uploaded file
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                // Log error or throw
                throw new ApplicationException("Blob upload failed: " + ex.Message);
            }
        }
    }
}
