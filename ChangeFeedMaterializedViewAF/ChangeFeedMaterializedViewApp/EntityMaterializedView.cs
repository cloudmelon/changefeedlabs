using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFeedMaterializedViewAF
{
    public class EntityMaterializedView
    {
        [JsonProperty("id")]
        public Guid Id;

        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("itemId")]
        public int ItemId;

        [JsonProperty("contentId")]
        public int ContentId;

        [JsonProperty("eventType")]
        public string EventType;

        [JsonProperty("orderId")]
        public int? OrderId;

        [JsonProperty("region")]
        public string Region;

        [JsonProperty("sessionId")]
        public string SessionId;

        [JsonProperty("PartitionId")]
        public int PartitionId;

        [JsonProperty("timestamp")]
        public string Timestamp;

        [JsonProperty("newval")]
        public string NewVal;
    }
}
