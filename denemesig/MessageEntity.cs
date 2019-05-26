using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace denemesig
{
    public class MessageEntity : TableEntity
    {
        public string ConnectionID { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public MessageEntity()
        {
        }

        public MessageEntity(string connectionID, string username, string message, DateTime createdAt)
        {
            this.ConnectionID = connectionID;
            this.Username = username;
            this.Message = message;
            this.CreatedAt = createdAt;
            PartitionKey = DateTime.Now.Ticks.ToString();
            RowKey = username;
        }

        public override string ToString()
        {
            return this.ConnectionID + " " + this.Username + " " + this.Message + " " + this.CreatedAt;
        }
    }
}
