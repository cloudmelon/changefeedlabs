using CosmosDB.Cache.Common.Models;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB.Cache.APIs.Services
{
    public interface IRequestService
    {
        Task<Document> CreateRequestAsync(Request doc);
        Task<Request> GetRequestByIdAsync(string id);
        Task<IEnumerable<Request>> GetRequestsAsync();
        Task<Document> UpdateRequestAsync(string id, Request doc);
        Task DeleteRequestAsync(string id);
    }
}
