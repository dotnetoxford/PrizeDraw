using Azure;
using Azure.Data.Tables;

namespace PrizeDraw.ZoomWebhook.Api.Models
{
    public class Attendee : ITableEntity
    {
        // ITableEntity properties...
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Our properties...

        public int Type { get; set; }
        public string MeetingUuid { get; set; }

        public string MeetingId { get; set; }
        public string MeetingName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserUuid { get; set; }
        public DateTime? JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}