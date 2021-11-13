using BitlyAssistant.Api.Requests;
using BitlyAssistant.DataAccess.Bitly;
using BitlyAssistant.Shared.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BitlyAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitlyAssistantController : ControllerBase
    {
        private const string API_KEY = "ed9381d5eb18d9e0c5f9a78870047d3b95905345";
        private IBitlyApiMiddleware _bitly;
        private HttpClient _httpClient;

        public BitlyAssistantController()
        {
            _httpClient = new HttpClient();
            _bitly = new BitlyApiMiddleware(_httpClient);
        }

        // GET: api/<BitlyAssistantController>
        [HttpPost]
        public IEnumerable<string> Post([FromBody] ShortenUrlRequest request)
        {
            var res = _bitly.ShortenUrl(request.Url, request.Domain);
            return new string[] { "value1", "value2" };
        }
    }
}
