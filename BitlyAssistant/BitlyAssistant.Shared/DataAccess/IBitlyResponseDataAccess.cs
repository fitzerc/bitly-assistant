using BitlyAssistant.Shared.Models;

namespace BitlyAssistant.Shared.DataAccess
{
    public interface IBitlyResponseDataAccess
    {
        int WriteBitlyResponse(string responseString);
        BitlyResponseModel ReadBitlyResponse(int respId);
    }
}
