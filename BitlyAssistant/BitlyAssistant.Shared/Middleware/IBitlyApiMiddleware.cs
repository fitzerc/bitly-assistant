using BitlyAssistant.Shared.Models;

namespace BitlyAssistant.Shared.Middleware
{
    public interface IBitlyApiMiddleware
    {
        string LastRequest { get; }
        ShortenUrlResponse ShortenUrl(string url, string domain);
        string GetGroupGuid();
    }
}
