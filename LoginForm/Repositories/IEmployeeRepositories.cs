
using ERP.Models;
using LoginForm.Data;
using LoginForm.Models;
using Microsoft.Data.SqlClient;

namespace LoginForm.Repositories
{
    public interface IEmployeeRepositories
    {
        //-----------  Login ----------------------
        bool Login(string Email,string Password);

        //-----------  Employee -------------------
        bool AddDept(departments departments);
        IEnumerable<departments> GetDept();

        bool DeleteDept(int id);

        bool AddRole(roles roles);

        IEnumerable<roles> GetRoles();

        bool DeleteRole(int id);
        bool AddEmp(employees employees, string selectedHobbies, byte[] profileimg);
        bool UpdateEmp(employees employees, string selectedHobbies, byte[] profileimg);
        bool DeleteEmp(int id);

        List<employees> GetAllEmployees();

        employees GetEmployeesById(int id);

        // Get Role To Give Access Rights on Role Basis
        employees getSessionData(string Email);

        // Get List Of Roles


        // --------------  FAQ -------------------------------
        bool AddFaq(FAQ Faq);
        List<FAQ> DispFaq();

        // -------------- Employee Leave -------------------------------
        bool AddLeaveRequests(leave_requests leave_Request);

        List<leave_requests> DisplayLeaveRequests();

        bool UpdateLeaveRequests(leave_requests leave_Requests);

      
        List<feedback> DisplayFeedback();

        employee_attendence GetTodayAttendence(int id);

        bool PunchIN(employee_attendence employee_Attendence);
        bool PunchOUT(employee_attendence employee_Attendence);

        List<employee_attendence> PunchHistory(int EmployeeID);


        // Add Campaign
        bool AddCampaign(campaign campaign, byte[] campaignIMG);

        List<campaign> DispCampaign();

        // Handloom
        bool AddHandloom(Handloom handloom);

        Handloom GetHandloomByID(int id);

        bool UpdateHandloom(Handloom handloom);
        bool DeleteHandloom(int id);

        List<Handloom> GetAllHandloom();

        // Customers
        bool DeleteCustomers(int id);

        List<Customer> DispCustomers();

        // Employee Salary Payout

        public List<PayableSalaryViewModel> GetMonthlyPayrollData();
        public void PayEmployeeSalary(int employeeId, decimal amount);
        public void PayAllSalaries();



    }
}
