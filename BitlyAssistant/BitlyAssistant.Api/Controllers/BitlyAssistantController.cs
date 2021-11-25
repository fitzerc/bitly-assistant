using BitlyAssistant.Api.Requests;
using BitlyAssistant.DataAccess.Bitly;
using BitlyAssistant.DataAccess.Postgres;
using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.Shared.DataAccess;
using BitlyAssistant.Shared.Middleware;
using BitlyAssistant.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BitlyAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitlyAssistantController : ControllerBase
    {
        private IBitlyApiMiddleware _bitly;
        private IBitlyRequestDataAccess _dbBitlyRequestAccess;
        private IBitlyResponseDataAccess _dbBitlyResponseAccess;
        private IShortLinkDataAccess _dbShortLinkDataAccess;

        public BitlyAssistantController(IConfiguration config,
            IBitlyApiMiddleware api,
            IBitlyRequestDataAccess bitlyRequestAccess,
            IBitlyResponseDataAccess bitlyResponseAccess,
            IShortLinkDataAccess shortLinkAccess)
        {
            _bitly = api;

            _dbBitlyRequestAccess = bitlyRequestAccess;
            _dbBitlyResponseAccess = bitlyResponseAccess;
            _dbShortLinkDataAccess = shortLinkAccess;
        }

        [HttpGet]
        public IEnumerable<ShortLinkModel> Get()
        {
            return _dbShortLinkDataAccess.ReadAllShortLinks();
        }

        // GET api/ShortLinkController/5
        [HttpGet("{id}")]
        public ShortLinkModel Get(int id)
        {
            var res = _dbShortLinkDataAccess.ReadShortLink(id);

            return res == null
                ? null
                : res;
        }

        // POST api/ShortLinkController
        [HttpPost]
        public ShortLinkModel Post([FromBody] ShortenUrlRequest req)
        {
            var bitlyResponse = RequestLinkFromBitly(req);
            if (bitlyResponse == null)
            {
                return null;
            }

            var shortRequest = _bitly.LastRequest;
            var shortRequestId = _dbBitlyRequestAccess.WriteBitlyRequest(shortRequest);
            var shortResponseId = _dbBitlyResponseAccess.WriteBitlyResponse(JsonConvert.SerializeObject(bitlyResponse));

            var tempShortLink = new ShortLinkModel()
            {
                short_link = bitlyResponse.link,
                long_link = bitlyResponse.long_url,
                request_id = shortRequestId,
                response_id = shortResponseId,
                description = req.Description,
            };

            var newShortLink = WriteShortLink(tempShortLink);

            return newShortLink;
        }

        private ShortenUrlResponse RequestLinkFromBitly(ShortenUrlRequest req)
        {
            if (string.IsNullOrEmpty(req.Url) || string.IsNullOrEmpty(req.Domain)) {
                return null;
            }

            return _bitly.ShortenUrl(req.Url, req.Domain);
        }

        public ShortLinkModel WriteShortLink(ShortLinkModel value)
        {
            var id = _dbShortLinkDataAccess.WriteShortLink(value);

            if (id == 0)
            {
                return null;
            }

            return _dbShortLinkDataAccess.ReadShortLink(id);
        }
    }
}
