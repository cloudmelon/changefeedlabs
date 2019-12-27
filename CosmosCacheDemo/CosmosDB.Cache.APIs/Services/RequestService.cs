using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDB.Cache.Common.Models;
using CosmosDB.Cache.Common.Repository;

namespace CosmosDB.Cache.APIs.Services
{
    public class RequestService : IRequestService
    {
        private readonly ICosmosDBRepository<Request> _respository;
        public RequestService(ICosmosDBRepository<Request> respository)
        {
            _respository = respository;
        }
        public async Task<Document> CreateRequestAsync(Request doc)
        {
            return await _respository.CreateRequestAsync(doc);
        }


        public async Task<Request> GetRequestByIdAsync(string id)
        {
            return await _respository.GetRequestByIdAsync(id);
        }

        public async Task<IEnumerable<Request>> GetRequestsAsync()
        {
            return await _respository.GetRequestsAsync();
        }

        public async Task<Document> UpdateRequestAsync(string id, Request doc)
        {
            return await _respository.UpdateRequestAsync(id, doc);
        }

        public async Task DeleteRequestAsync(string id)
        {
            await _respository.DeleteRequestAsync(id);
        }

    }

}
