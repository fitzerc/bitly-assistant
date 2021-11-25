namespace BitlyAssistant.Api.Requests
{
    public class ShortenUrlRequest
    {
        public string Url { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; } = "";
    }
}
