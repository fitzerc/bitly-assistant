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
        private const string API_KEY = "";
        private const string GROUP_GUID = "";
        private readonly HttpClient _httpClient;
        public BitlyApiMiddleware(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
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

        public ShortenedUrlResponse ShortenUrl(string url, string domain)
        {
            var response = ShortenUrlAsync(url, domain).Result;

            return JsonConvert.DeserializeObject<ShortenedUrlResponse>(response);
        }

        private async Task<string> ShortenUrlAsync(string url, string domain)
        {
            url = "\"" + url + "\"";
            domain = "\"" + domain + "\"";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            var values = "{\"group_guid\": " + GROUP_GUID
                + ", \"domain\": " + domain
                + ", \"long_url\": " + url
                + "}";
            var content = new StringContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync("https://api-ssl.bitly.com/v4/shorten", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
