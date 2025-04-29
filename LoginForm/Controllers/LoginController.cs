using ERP.Repositories;
using ERP.Controllers;
using ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ERP.Controllers
{
    public class LoginController : Controller
    {
        private readonly IEmployeeRepositories _repositories;
        public LoginController(IEmployeeRepositories repositories)
        {
            _repositories = repositories;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {

            if (_repositories.Login(Email, Password))
            {
                // Login successful

                //Session["UserEmail"] = ;


                var employee = _repositories.getSessionData(Email); // This will return an employee object
                int role = employee.role_id;
                string Name = employee.Name;
                // Assuming Role is a string property in the 'employees' class
                int EmployeeID = employee.EmployeeID; // Assuming Role is a string property in the 'employees' class

                // Check if role is null or not before setting the session
                if (role != null)
                {
                    HttpContext.Session.SetString("role", role.ToString());
                    HttpContext.Session.SetString("EmployeeID", EmployeeID.ToString());
                    HttpContext.Session.SetString("Email", Email);
                    HttpContext.Session.SetString("Name", Name);
                }
                else
                {
                    // Handle the case where role is not found, maybe log the issue
                    ViewBag.ErrorMessage = "User role not found.";
                    return View();
                }
                return RedirectToAction("Home", "AdminHome");
            }
            else
            {
                // Login failed
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
        }


        public IActionResult AddCampaign()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddCampaign(campaign cam, IFormFile campaignIMG)
        {

            if (campaignIMG != null && campaignIMG.Length > 0)
            {
                //string fileName = Path.GetFileName(campaignIMG.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                // Save the file to the specified path
                string fileName = Path.GetFileName(campaignIMG.FileName);
                string filePathWithName = Path.Combine(filePath, fileName);


                using (var stream = new FileStream(filePathWithName, FileMode.Create))
                {
                    campaignIMG.CopyTo(stream);
                }
            }
            cam.campaignIMG = campaignIMG.FileName;
            bool res = _repositories.AddCampaign(cam);
            if (res)
            {
                return RedirectToAction("DispCampaign");
            }

            return View(cam);
        }

        public IActionResult DispCampaign()
        {
            var campaigns = _repositories.DispCampaign();
            return View(campaigns);
        }

        [HttpPost]
        public IActionResult EditCampaignStatus(int campaign_id)
        {
            var campaigns = _repositories.EditCampaignStatus(campaign_id);
            if (campaigns)
            {
                return RedirectToAction("DispCampaign");
            }

            return View(campaigns);
        }
    }
}
