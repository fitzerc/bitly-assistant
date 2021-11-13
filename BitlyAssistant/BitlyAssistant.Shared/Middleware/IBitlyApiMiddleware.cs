using BitlyAssistant.Shared.Models;

namespace BitlyAssistant.Shared.Middleware
{
    public interface IBitlyApiMiddleware
    {
        ShortenedUrlResponse ShortenUrl(string url, string domain);
        string GetGroupGuid();
    }
}
