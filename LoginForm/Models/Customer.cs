using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginForm.Models
{
    //[Table("customers")]
    public class Customer
    {
        public int customer_id { get; set; }
        //[Required(ErrorMessage = "First Name is required.")]

        public string firstName { get; set; }
        //[Required(ErrorMessage = "Last Name is required11.")]

        public string lastName { get; set; }
       // [Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email address.")]
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}
