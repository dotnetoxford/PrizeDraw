namespace PrizeDraw.Models.MeetupCom.Rsvps
{
    public class Member
    {
        public int id { get; set; }
        public string name { get; set; }
        public Photo photo { get; set; }
        public Event_Context event_context { get; set; }
        public string role { get; set; }
        public string bio { get; set; }
    }
}