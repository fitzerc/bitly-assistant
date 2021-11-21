using BitlyAssistant.Shared.Models;
using System.Collections.Generic;

namespace BitlyAssistant.Shared.DataAccess
{
    public interface IShortLinkDataAccess
    {
        int WriteShortLink(ShortLinkModel link);
        ShortLinkModel ReadShortLink(int linkId);
        List<ShortLinkModel> ReadAllShortLinks();
    }
}
