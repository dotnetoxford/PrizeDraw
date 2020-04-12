using System;

namespace PrizeDraw.ZoomFunctions.Models
{
    public class ParticipantJoinedAndLeftEventData
    {
        public string @event { get; set; }
        public Payload payload { get; set; }

        public class Payload
        {
            public string account_id { get; set; }
            public Object @object { get; set; }

            public class Object
            {
                public int duration { get; set; }
                public DateTime start_time { get; set; }
                public string timezone { get; set; }
                public string topic { get; set; }
                public string id { get; set; }
                public int type { get; set; }
                public string uuid { get; set; }
                public Participant participant { get; set; }
                public string host_id { get; set; }

                public class Participant
                {
                    public string id { get; set; }
                    public string user_id { get; set; }
                    public string user_name { get; set; }
                    public DateTime? join_time { get; set; }
                    public DateTime? leave_time { get; set; }
                }
            }
        }
    }
}