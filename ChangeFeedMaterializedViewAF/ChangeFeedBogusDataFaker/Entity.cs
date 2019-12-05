using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeFeedBogusDataFaker
{
    public class Entity
    {
        public Guid id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int ContentId { get; set; }
        public string EventType { get; set; }
        public int OrderId { get; set; }
        public string Region { get; set; }
        public string SessionId { get; set; }
        public int PartitionId { get; set; }
        public string Timestamp { get; set; }
    }
}
