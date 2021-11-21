using BitlyAssistant.Shared.Models;

namespace BitlyAssistant.Shared.DataAccess
{
    public interface IBitlyRequestDataAccess
    {
        int WriteBitlyRequest(string requestString);
        BitlyRequestModel ReadBitlyRequest(int reqId);
    }
}
