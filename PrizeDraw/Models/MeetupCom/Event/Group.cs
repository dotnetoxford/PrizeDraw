namespace PrizeDraw.Models.MeetupCom.Event
{
    public class Group
    {
        public long created { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string join_mode { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string urlname { get; set; }
        public string who { get; set; }
        public string localized_location { get; set; }
        public string region { get; set; }
    }
}