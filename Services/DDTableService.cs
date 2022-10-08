namespace DDAzure{
    using Azure.Data.Tables;
    using System.Collections.Generic;

    //https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/tables/Azure.Data.Tables/samples/Sample2CreateDeleteEntities.md
    public class DDTableService
    {
        private TableClient _tableClient;
        private string _connectionString;
        
        public DDTableService(string connectionString, string tableName)
        {
            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }
   

        public void InsertEntity(TableEntity e)
        {
            _tableClient.AddEntity(e);
        }

        public void DeleteEntity(string partitionKey, string rowKey)
        {
            _tableClient.DeleteEntity(partitionKey, rowKey);
        }
        public void UpdateEntity(TableEntity p)
        {
       //   _tableClient.UpdateEntity(p);
        }
 


    }
}