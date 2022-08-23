// namespace DDControllers
// {
//     public class BlobController
//     {
//         private string containerName;
//         private string _connectionString;
//         private DDBlobService _service;

//         public BlobController(string ConnectionString)
//         {
//             if(ConnectionString == null)
//             {
//                 throw new ArgumentNullException("ConnectionString");
//             } else{
//                 this._connectionString = ConnectionString;
//                 this._service = new DDBlobService(ConnectionString);
//             }   
//         }

//         public void CreateContainer(string containerName)
//         {
//             this.containerName = containerName;
//             this._service.CreateContainer(containerName);
//         }

//         public void UploadBlob(string fileName)
//         {
//             this._service.UploadBlob(fileName);
//         }

     

//         public void DeleteBlob(string fileName)
//         {
//             this._service.DeleteBlob(fileName);
//         }

     

//     }
// }
