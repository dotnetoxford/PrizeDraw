using System;
using Microsoft.Azure.CosmosDB.Table;

namespace PrizeDraw.Models
{
    public class Attendee : TableEntity
    {
        public Attendee(string meetingId)
        {
            PartitionKey = meetingId;
            RowKey = Guid.NewGuid().ToString();
        }

        public Attendee()
        {
        }

        public int Type { get; set; }
        public string MeetingUuid { get; set; }

        public string MeetingName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserUuid { get; set; }
        public DateTime? JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}