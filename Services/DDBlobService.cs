namespace DDAzure
{
    using Azure.Storage.Blobs;
    using System.Collections.Generic;

    public class DDBlobService
    {
        private BlobContainerClient _blobContainer;
        private BlobServiceClient _blobService;
        private string _connectionString;

        public DDBlobService(string connectionString)
        {
            _blobService = new BlobServiceClient(connectionString);
        }

        public void CreateContainer(string containerName)
        {
            _blobContainer = _blobService.GetBlobContainerClient(containerName);
            _blobContainer.CreateIfNotExists();
        }

        public void UploadBlob(string fileName)
        {
            string blobName = fileName;
            BlobClient blobClient = _blobContainer.GetBlobClient(blobName);
            blobClient.Upload(fileName);
        }

        public void DeleteBlob(string fileName)
        {
            BlobClient blobClient = _blobContainer.GetBlobClient(fileName);
            blobClient.Delete();
        }

        public void DeleteContainer(string containerName)
        {
            _blobContainer = _blobService.GetBlobContainerClient(containerName);
            _blobContainer.Delete();
        }

         public List<string> GetInfoFromBlobsInContainer()
         {
            var blobs = _blobContainer.GetBlobs();
            List<string> blobInfos = new List<string>();
            foreach(var blob in blobs)
            {
                blobInfos.Add($"{blob.Name} --> Created On: {blob.Properties.CreatedOn:yyyy-MM-dd HH:mm:ss} Size: {blob.Properties.ContentLength}");
            }
            return blobInfos;
     }
    }
}