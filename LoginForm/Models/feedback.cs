namespace LoginForm.Models
{
    public class feedback
    {
       public int feedback_id { get; set; }
       public int customer_id { get; set; }
       public int productid { get; set; }
       public decimal rating { get; set; }
       public string comments { get; set; }
       public DateTime submitted_at { get; set; }

    }
}
