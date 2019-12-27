using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB.Cache.Common.Models
{
    public class Request
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "itemId")]
        public int ItemId { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public int? OrderId { get; set; }

        [JsonProperty(PropertyName = "contentId")]
        public int? ContentId { get; set; }

        [JsonProperty(PropertyName = "sessionId")]
        public string SessionId { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "Region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "Table")]
        public string Table { get; set; }

    }
}
