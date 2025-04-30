namespace ERP.Models
{
    public class PayableSalaryViewModel
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public decimal MonthlySalary { get; set; }
        public int PresentDays { get; set; }
        public int TotalDaysInMonth { get; set; }
        public decimal PayableAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
