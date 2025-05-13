namespace ERP.Models
{
    public class campaign
    {
        public int campaign_id { get; set; }
        public string campaign_name { get; set; }
        public string campaign_type { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string campaignIMG { get; set; }
        public decimal discount { get; set; }
        public string status { get; set; }


    }
}
