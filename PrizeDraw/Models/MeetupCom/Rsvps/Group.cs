namespace PrizeDraw.Models.MeetupCom.Rsvps
{
    public class Group
    {
        public int id { get; set; }
        public string urlname { get; set; }
        public string name { get; set; }
        public string who { get; set; }
        public int members { get; set; }
        public string join_mode { get; set; }
        public Group_Photo group_photo { get; set; }
    }
}