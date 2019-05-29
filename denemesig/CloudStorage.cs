using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace denemesig
{
    /// <summary>
    /// Cloud storage class.
    /// </summary>
    public class CloudStorage : ICloudStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:denemesig.CloudStorage"/> class.
        /// Default constructor of CloudStorage class.
        /// </summary>
        public CloudStorage()
        {
        }

        /// <summary>
        /// Inserts the message to Azure Cloud Storage Account Table "denemesigmessage"
        /// Creates the table in cloud if it doesn't exist.
        /// </summary>
        /// <returns>The message <see cref="T:denemesig.MessageEntity"/> class.</returns>
        /// <param name="connectionID">Connection identifier.</param>
        /// <param name="user">User.</param>
        /// <param name="message">Message.</param>
        public MessageEntity InsertMessage(string connectionID, string user, string message)
        {
            if (string.IsNullOrEmpty(connectionID))
                throw new ArgumentException("connection ID parameter is null or empty");
            if (string.IsNullOrEmpty(user))
                throw new ArgumentException("user parameter is null or empty");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("message parameter is null or empty");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=denemesigstorage;AccountKey=9Rr4AuGxYzS2veA5eYxblu/gobva4A048xbrk/2g03FHnewEY70vB1xfaj0QvSTRY8zE3ezc7uFJ7a4/aAgGvQ==;EndpointSuffix=core.windows.net");
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("denemesigmessage");

            table.CreateIfNotExistsAsync();
            MessageEntity messageEntity = new MessageEntity(connectionID, user, message, DateTime.Now);
            TableOperation insertOp = TableOperation.Insert(messageEntity);
            table.ExecuteAsync(insertOp);

            return messageEntity;
        }
    }
}
