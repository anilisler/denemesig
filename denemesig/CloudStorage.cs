using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace denemesig
{
    public class CloudStorage : ISave
    {
        public CloudStorage()
        {
        }

        public async Task InsertMessage(string connectionID, string user, string message)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=denemesigstorage;AccountKey=9Rr4AuGxYzS2veA5eYxblu/gobva4A048xbrk/2g03FHnewEY70vB1xfaj0QvSTRY8zE3ezc7uFJ7a4/aAgGvQ==;EndpointSuffix=core.windows.net");
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("denemesigmessage");

            await table.CreateIfNotExistsAsync();
            MessageEntity messageEntity = new MessageEntity(connectionID, user, message, DateTime.Now);
             TableOperation insertOp = TableOperation.Insert(messageEntity);
            await table.ExecuteAsync(insertOp);

            //return client.GetTableReference("connection");
        }
    }
}
