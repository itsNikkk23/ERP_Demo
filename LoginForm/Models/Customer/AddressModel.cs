namespace ERP.Models
{
    public class AddressModel
    {
        public int AddressID { get; set; }
        public int CustomerId { get; set; }
        public string HouseNo { get; set; }
        public string Street { get; set; }
        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string Pincode { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
