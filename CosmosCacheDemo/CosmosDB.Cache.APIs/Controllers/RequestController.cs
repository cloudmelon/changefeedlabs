using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDB.Cache.APIs.Services;
using CosmosDB.Cache.Common.Helpers;
using CosmosDB.Cache.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CosmosDB.Cache.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly CacheHelper _cache;
        private readonly IRequestService _service;
        public RequestController(CacheHelper cache, IRequestService service)
        {
            _cache = cache;
            _service = service;
        }

        // GET api/requests
        [HttpGet]
        public async Task<IEnumerable<Request>> GetAllRequests()
        {
            var results = await _service.GetRequestsAsync(); 


            var cacheId = "request";

            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            };
           

            var cache = _cache.ToByteArray(JsonConvert.SerializeObject(results));
          
            await _cache.SetAsync(cacheId, cache, options);

            var requestCache = await _cache.GetAsync(cacheId);

            var output = _cache.FromByteArray<string>(requestCache);

            var requests = JsonConvert.DeserializeObject<IEnumerable<Request>>(output);

            return requests;
        }

        // GET api/requests/5
        [HttpGet("{id}")]
        public async Task<Request> GetRequestById(string id)
        {
            return (_cache.IsCacheExist(id))? _cache.FromByteArray<Request>(await _cache.GetAsync(id)) : await _service.GetRequestByIdAsync(id);
        }


        // POST api/requests
        [HttpPost]
        public async void CreateRequest([FromBody] Request request)
        {
            await _service.CreateRequestAsync(request);
        }

        // PUT api/requests5
        [HttpPut("{id}")]
        public async void UpdateRequest(string id, [FromBody] Request request)
        {
            await _service.UpdateRequestAsync(id, request);
        }

        // DELETE api/requests/5
        [HttpDelete("{id}")]
        public async void DeleteRequest(string id)
        {
            await _service.DeleteRequestAsync(id);
        }
    }
}
