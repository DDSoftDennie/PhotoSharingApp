namespace DDAzure{
    using Azure.Data.Tables;
    using System.Collections.Generic;

    //https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/tables/Azure.Data.Tables/samples/Sample2CreateDeleteEntities.md
    public class DDTableService
    {
        private TableClient _tableClient;
        private string _connectionString;
        
        public DDTableService()
        {

        }



        public async Task<bool> CreateTable(string tableName)
        {
            try{
          
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
           

        }


        public void DeleteTable(string tableName)
        {
           
        }
        public void InsertEntity(string tableName, TableEntity p)
        {
           
        }

        public void DeleteEntity(string tableName, string partitionKey, string rowKey)
        {
     
        
        }
        public void UpdateEntity(string tableName, TableEntity p)
        {
          
        }
 


    }
}