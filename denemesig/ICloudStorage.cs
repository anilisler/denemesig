namespace denemesig
{
    public interface ICloudStorage
    {
        MessageEntity InsertMessage(string connectionID, string user, string message);
    }
}
