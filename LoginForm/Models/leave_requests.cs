using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class leave_requests
    {
        [Key]
        public int LeaveID { get; set; }

        [ForeignKey("employees")]
        public int EmployeeID { get; set; }
        public employees? employees { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public DateTime Applied_at { get; set; }

    }
}
