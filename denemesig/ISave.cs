using System.Threading.Tasks;

namespace denemesig
{
    public interface ISave
    {
        Task InsertMessage(string connectionID, string user, string message);
    }
}
