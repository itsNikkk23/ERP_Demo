using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginForm.Models
{
    public class employee_attendence
    {
        [Key]
        public int AttendenceID { get; set; }

        [ForeignKey("employees")]
        public int EmployeeID { get; set; }
        public employees? employees { get; set; }
        
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public DateTime? ShiftStart { get; set; }   // Nullable DateTime for ShiftStart
        public DateTime? ShiftEnd { get; set; }     // Nullable DateTime for ShiftEnd
    }
}
