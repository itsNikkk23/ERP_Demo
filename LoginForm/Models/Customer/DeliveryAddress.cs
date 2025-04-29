namespace ERP.Models
{
    public class DeliveryAddress
    {
        public int addressID { get; set; }
        public int customer_id { get; set; }
        public string houseNo { get; set; }
        public string street { get; set; }
        public int cityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public int pincode { get; set; }
    }
}
