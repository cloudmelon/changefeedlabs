using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFeedMaterializedViewAF
{
    public class Entity
    {
        public Guid Id;
        public int UserId;
        public int ItemId;
        public int ContentId;
        public string EventType;
        public int OrderId;
        public string Region;
        public string SessionId;
        public int PartitionId;
        public string Timestamp;

        public double Value;
        public static Entity FromDocument(Document document)
        {
            var result = new Entity()
            {
                Id = document.GetPropertyValue<Guid>("id"),
                UserId = document.GetPropertyValue<int>("userId"),
                ItemId = document.GetPropertyValue<int>("itemId"),
                ContentId = document.GetPropertyValue<int>("contentId"),
                EventType = document.GetPropertyValue<string>("eventType"),
                OrderId = document.GetPropertyValue<int>("orderId"),
                Region = document.GetPropertyValue<string>("region"),
                SessionId = document.GetPropertyValue<string>("sessionId"),
                PartitionId = document.GetPropertyValue<int>("partitionId"),
                Timestamp = document.GetPropertyValue<DateTime>("timestamp").ToString("yyyy-MM-ddTHH:mm:ssK")
            };

            return result;
        }
    }
}
