using ERP.Models;
using ERP.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.Controllers.Admin
{
    //[Route("Home")]
    public class AdminHome : Controller
    {
        private readonly IEmployeeRepositories _repositories;
        public AdminHome(IEmployeeRepositories repositories)
        {
            _repositories = repositories;
        }

        //[Route("")]
        public IActionResult Home()
        {
            return View();
        }

        //Store
        public IActionResult DispHandlooms()
        {

            var handlooms = _repositories.GetAllHandloom();
            return View(handlooms);
        }

        public IActionResult AddHandloom()
        {
           
            return View(new Handloom());
        }

        [HttpPost]
        public IActionResult AddHandloom(Handloom handloom)
        {
           
            bool res = _repositories.AddHandloom(handloom);
            if (res)
            {
                return RedirectToAction("DispHandlooms");
            }
           
            Console.WriteLine("Model validation failed:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
           
            return View(handloom);

           
        }
        public IActionResult DeleteHandloom(int id)
        {
            _repositories.DeleteHandloom(id);
            return RedirectToAction("DispHandlooms");
        }

        public IActionResult UpdateHandloom(int id)
        {
          
            var handloom = _repositories.GetHandloomByID(id);
            if (handloom == null)
            {
                return NotFound();
            }

            return View(handloom);
        }

        [HttpPost]
        public IActionResult UpdateHandloom(Handloom handloom)
        {
           
            bool result = _repositories.UpdateHandloom(handloom);
            if (result)
            {
                return RedirectToAction("DispHandlooms");
            }
            return View(handloom);
           
        }

        // Bulk Puchase
        public IActionResult BulkPurchase()
        {
            return View();
        }

        //Customers
        public IActionResult DispCustomers()
        {

            var customers = _repositories.DispCustomers();
            return View(customers);
        }

        public IActionResult DeleteCustomers(int id)
        {
            _repositories.DeleteCustomers(id);
            return RedirectToAction("DispCustomers");
        }
    }
}

