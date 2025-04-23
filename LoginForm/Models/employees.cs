using LoginForm.Models;
using System.ComponentModel.DataAnnotations;

namespace LoginForm.Models
{
    public class employees
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int role_id { get; set; }
        public roles? roles { get; set; }

        public int DepartmentID { get; set; }
        public departments? departments { get; set; }

        public decimal Salary { get; set; }
        public List<string> Hobbies { get; set; }

        public byte[] profileimg { get; set; }

        public string contenttype { get; set; }
    }
}
