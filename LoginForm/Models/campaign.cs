namespace LoginForm.Models
{
    public class campaign
    {
        public int campaign_id { get; set; }
        public string campaign_name { get; set; }
        public string campaign_type { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public byte[] campaignIMG { get; set; }
       
    }
}
