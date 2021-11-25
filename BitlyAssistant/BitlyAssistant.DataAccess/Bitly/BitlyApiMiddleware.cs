using BitlyAssistant.Shared.Middleware;
using BitlyAssistant.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BitlyAssistant.DataAccess.Bitly
{
    public class BitlyApiMiddleware : IBitlyApiMiddleware
    {
        private readonly string _apiKey;
        private readonly string _groupGuid;
        private readonly HttpClient _httpClient;

        private string _lastRequest = "";

        string IBitlyApiMiddleware.LastRequest => _lastRequest;

        public BitlyApiMiddleware(HttpClient httpClient, string apiKey, string groupGuid)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _groupGuid = groupGuid;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public string GetGroupGuid()
        {
            return GetGroupGuidAsync().Result;
        }
        private async Task<string> GetGroupGuidAsync()
        {
            var response = await _httpClient.GetAsync("https://api-ssl.bitly.com/v4/groups");
            return await response.Content.ReadAsStringAsync();
        }

        public ShortenUrlResponse ShortenUrl(string url, string domain)
        {
            var response = ShortenUrlAsync(url, domain).Result;

            return JsonConvert.DeserializeObject<ShortenUrlResponse>(response);
        }

        private async Task<string> ShortenUrlAsync(string url, string domain)
        {
            url = "\"" + url + "\"";
            domain = "\"" + domain + "\"";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            var request = _lastRequest = "{\"group_guid\": " + _groupGuid
                + ", \"domain\": " + domain
                + ", \"long_url\": " + url
                + "}";

            //TODO: Write request

            var content = new StringContent(request);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync("https://api-ssl.bitly.com/v4/shorten", content);

            //TODO: Write response

            return await response.Content.ReadAsStringAsync();
        }
    }
}
