namespace denemesig
{
    /// <summary>
    /// Interface of Cloud storage.
    /// </summary>
    public interface ICloudStorage
    {
        /// <summary>
        /// Declaration of Inserts the message method.
        /// </summary>
        /// <returns>The message.</returns>
        /// <param name="connectionID">Connection identifier.</param>
        /// <param name="user">User.</param>
        /// <param name="message">Message.</param>
        MessageEntity InsertMessage(string connectionID, string user, string message);
    }
}
