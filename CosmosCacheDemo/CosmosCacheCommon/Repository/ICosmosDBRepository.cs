using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CosmosDB.Cache.Common.Repository
{
    public interface ICosmosDBRepository<T> where T : class
    {
        Task<Document> CreateRequestAsync(T doc);
        Task<T> GetRequestByIdAsync(string id);
        Task<IEnumerable<T>> GetRequestsAsync();
        Task<Document> UpdateRequestAsync(string id, T doc);
        Task DeleteRequestAsync(string id);
       
    }
}
