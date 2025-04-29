using System.Diagnostics.Eventing.Reader;

namespace ERP.Models
{
    public class payroll
    {
       public int Payroll_ID { get; set; }
       public int EmployeeID { get; set; }
       public decimal Amount { get; set; }
       public DateTime PayDate { get; set; }
       public string PayType { get; set; }
        
    }
}
