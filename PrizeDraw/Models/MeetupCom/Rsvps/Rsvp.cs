namespace PrizeDraw.Models.MeetupCom.Rsvps
{
    public class Rsvp
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string response { get; set; }
        public int guests { get; set; }
        public Event _event { get; set; }
        public Group group { get; set; }
        public Member member { get; set; }
        public Venue venue { get; set; }
    }
}