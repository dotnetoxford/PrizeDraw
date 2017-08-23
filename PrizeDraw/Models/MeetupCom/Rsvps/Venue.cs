namespace PrizeDraw.Models.MeetupCom.Rsvps
{
    public class Venue
    {
        public int id { get; set; }
        public string name { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public bool repinned { get; set; }
        public string address_1 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string localized_country_name { get; set; }
    }
}