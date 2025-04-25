
using LoginForm.Models;
using LoginForm.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoginForm.Controllers
{
    public class employeeController : Controller
    {
        private readonly IEmployeeRepositories _repositories;
        public employeeController( IEmployeeRepositories repositories)
        {
            _repositories = repositories;
        }

        public IActionResult DisplayDept()
        {
            var depts = _repositories.GetDept();
            return View(depts);
        }

        public IActionResult AddDept()
        {
            return View(new departments());
        }

        [HttpPost]
        public IActionResult AddDept(departments departments)
        {
            bool res = _repositories.AddDept(departments);
            if (res)
            {
                
                return RedirectToAction("DisplayDept");
            }
            // return View(stud);

            // }
            Console.WriteLine("Model validation failed:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            ViewBag.department = _repositories.GetDept();
            ViewBag.course = _repositories.GetRoles();
            return View(departments);
        }
        public IActionResult DisplayRole()
        {
            var roles = _repositories.GetRoles();
            return View(roles);
        }
        public IActionResult DeleteDept(int id)
        {
            _repositories.DeleteDept(id);
            return RedirectToAction("DisplayDept");
        }

        public IActionResult AddRole()
        {
            return View(new roles());
        }

        [HttpPost]
        public IActionResult AddRole(roles roles)
        {
            bool res = _repositories.AddRole(roles);
            if (res)
            {

                return RedirectToAction("DisplayRole");
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
        public IActionResult DeleteRole(int id)
        {
            _repositories.DeleteRole(id);
            return RedirectToAction("DisplayRole");
        }

        [HttpGet]
        public IActionResult Index()
        {
             var employees = _repositories.GetAllEmployees(); // example
            return View(employees);
        }

        public IActionResult AddEmp()
        {
            ViewBag.roles = _repositories.GetRoles();
            ViewBag.depts = _repositories.GetDept();
            return View(new employees());
        }

        [HttpPost]
        public async Task<IActionResult> AddEmp(employees employees, IFormFile profileimg)
        {
            

            byte[] profileimg1 = null;
            if (profileimg != null && profileimg.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                   await profileimg.CopyToAsync(memoryStream);
                    profileimg1 = memoryStream.ToArray();
                    employees.contenttype = profileimg.ContentType;
                }
            }
            //  if (ModelState.IsValid)
            // {

            string hobby = employees.Hobbies != null ? string.Join(",", employees.Hobbies) : "";
            bool res = _repositories.AddEmp(employees, hobby, profileimg1);
            if (res)
            {
                return RedirectToAction("Index");
            }
            // return View(stud);

            // }
            Console.WriteLine("Model validation failed:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            ViewBag.department = _repositories.GetDept();
            ViewBag.course = _repositories.GetRoles();
            return View(employees);
        }
        public IActionResult DeleteEmp(int id)
        {
            _repositories.DeleteEmp(id);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateEmp(int id)
        {
            ViewBag.roles = _repositories.GetRoles();
            ViewBag.depts = _repositories.GetDept();
            var employees = _repositories.GetEmployeesById(id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        [HttpPost]
        public IActionResult UpdateEmp(employees employees, IFormFile profileimg)
        {
            byte[] profileimg1 = null;
            if (profileimg != null && profileimg.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    profileimg.CopyToAsync(memoryStream);
                    profileimg1 = memoryStream.ToArray();
                    employees.contenttype = profileimg.ContentType;
                }
            }
           /* if (ModelState.IsValid)
            {*/

                string selectedHobbies = employees.Hobbies != null ? string.Join(",", employees.Hobbies) : "";

                bool result = _repositories.UpdateEmp(employees, selectedHobbies, profileimg1);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View(employees);
           /* }
            return View(employees);*/
        }
    }
}
