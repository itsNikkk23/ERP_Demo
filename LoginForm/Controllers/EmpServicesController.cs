using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ERP.Controllers
{
    public class EmpServicesController : Controller
    {
        private readonly IEmployeeRepositories _repositories;
        public EmpServicesController(IEmployeeRepositories repositories)
        {
            _repositories = repositories;
        }
        public IActionResult Index()
        {
          
            return View();
        }

        public IActionResult DispFaq()
        {

            var Faq = _repositories.DispFaq();
            return View(Faq);
        }

        public IActionResult FAQ()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FAQ(FAQ Faq)
        {
            bool res = _repositories.AddFaq(Faq);
            if (res)
            {
                return RedirectToAction("DispFaq");
            }
            // return View(stud);

            // }
            Console.WriteLine("Model validation failed:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
           
            return View();
        }


        public IActionResult DisplayLeaveRequests()
        {
            var leave_requests = _repositories.DisplayLeaveRequests();
            return View(leave_requests);
        }

        [HttpPost]
        public IActionResult UpdateLeaveRequests(leave_requests leave_Requests)
            {
           

            bool result = _repositories.UpdateLeaveRequests(leave_Requests); // Call your database update function

            if (result)
            {
                // Optionally, redirect or send a success message
                return RedirectToAction("Index"); // Replace "Index" with your desired page
            }

            // Handle failure
            return View("Error"); // Replace with your error handling view
        }

        public IActionResult AddLeaveRequests()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLeaveRequests(leave_requests leave_Requests)
        {
            leave_Requests.EmployeeID = Convert.ToInt32(HttpContext.Session.GetString("EmployeeID"));
            bool res = _repositories.AddLeaveRequests(leave_Requests);
            if (res)
            {
                return RedirectToAction("DisplayLeaveRequests");
            }
            // return View(stud);

            // }
            Console.WriteLine("Model validation failed:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View();
        }

        public IActionResult GetTodayAttendence()
        {
            var employees_attendence = _repositories.GetTodayAttendence(Convert.ToInt32(HttpContext.Session.GetString("EmployeeID")));
            string Status = employees_attendence.Status; // Assuming Role is a string property in the 'employees' class
            HttpContext.Session.SetString("Status", Status.ToString());

            if (employees_attendence == null)
            {
                return NotFound();
            }

            return View(employees_attendence);
        }

        public IActionResult UpdateAttendence()
        {
            
            return View();

        }

        [HttpPost]
        public IActionResult UpdateAttendence(employee_attendence employee_Attendence)
        {
            string status = HttpContext.Session.GetString("Status");

            if (status == "punch in")
            {
                bool result = _repositories.PunchOUT(employee_Attendence);
                if (result)
                {

                    return RedirectToAction("GetTodayAttendence");
                }

            }
            else
            {
                bool result = _repositories.PunchIN(employee_Attendence);
                if (result)
                {

                    return RedirectToAction("GetTodayAttendence");
                }
            }
                return View("Error");
        }

        public IActionResult PunchHistory()
        {
            var employee_Attendences = _repositories.PunchHistory(Convert.ToInt32(HttpContext.Session.GetString("EmployeeID")));
            return View(employee_Attendences);
        }

        // payroll

        public ActionResult Payroll()
        {
            var model = _repositories.GetMonthlyPayrollData();
            return View(model);
        }

        [HttpPost]
        public ActionResult PayEmployee(int id, decimal amount)
        {
            _repositories.PayEmployeeSalary(id, amount);
            return RedirectToAction("Payroll");
        }

        [HttpPost]
        public ActionResult PayAll()
        {
            _repositories.PayAllSalaries();
            return RedirectToAction("Payroll");
        }
    }
}
