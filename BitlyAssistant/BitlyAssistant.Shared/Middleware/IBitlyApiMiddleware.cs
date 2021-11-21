using BitlyAssistant.Shared.Models;

namespace BitlyAssistant.Shared.Middleware
{
    public interface IBitlyApiMiddleware
    {
        ShortLinkModel ShortenUrl(string url, string domain);
        string GetGroupGuid();
    }
}
