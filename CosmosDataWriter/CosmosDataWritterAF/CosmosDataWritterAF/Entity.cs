using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDataWritterAF
{
    public class Entity
    {
        public Guid id { get; set; }
        public int? userId { get; set; }
        public int? itemId { get; set; }
        public int? contentId { get; set; }
        public string EventType { get; set; }
        public int? orderId { get; set; }
        public string region { get; set; }
        public string sessionId { get; set; }
        public string created { get; set; }
    }
}
