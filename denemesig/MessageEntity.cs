using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace denemesig
{
    /// <summary>
    /// MessageEntity is an entity class which represents my Azure Cloud Storage Table named "denemesigmessage".
    /// It's a subclass of TableEntity superclass which is Azure Storage Table entity, sets PartitionKey and RowKey.
    /// </summary>
    /// <remarks>
    /// Entity class representation of Azure Cloud Table named "denemesigmessage".
    /// </remarks>
    public class MessageEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the connection identifier.
        /// </summary>
        /// <value>The Client's connection identifier.</value>
        public string ConnectionID { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username of Client.</value>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message sent by Client.</value>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The date of message created at.</value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Default constructor of <see cref="T:denemesig.MessageEntity"/>
        /// Initializes a new instance of the <see cref="T:denemesig.MessageEntity"/> class.
        /// </summary>
        /// <remarks>
        /// Default constructor of <see cref="T:denemesig.MessageEntity"/>
        /// </remarks>
        public MessageEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:denemesig.MessageEntity"/> class.
        /// </summary>
        /// <param name="connectionID">Connection identifier.</param>
        /// <param name="username">Username.</param>
        /// <param name="message">Message.</param>
        /// <param name="createdAt">Created at.</param>
        public MessageEntity(string connectionID, string username, string message, DateTime createdAt)
        {
            this.ConnectionID = connectionID;
            this.Username = username;
            this.Message = message;
            this.CreatedAt = createdAt;
            PartitionKey = DateTime.Now.Ticks.ToString();
            RowKey = username;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:denemesig.MessageEntity"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:denemesig.MessageEntity"/>.</returns>
        public override string ToString()
        {
            return this.ConnectionID + " " + this.Username + " " + this.Message + " " + this.CreatedAt;
        }
    }
}
