namespace PrizeDraw.Models.MeetupCom.Rsvps
{
    public class Event
    {
        public string id { get; set; }
        public string name { get; set; }
        public int yes_rsvp_count { get; set; }
        public long time { get; set; }
        public int utc_offset { get; set; }
    }
}