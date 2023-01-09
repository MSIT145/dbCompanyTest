using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace dbCompanyTest.Controllers
{
    public class Staff_HomeController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        public IActionResult Index()
        {
           
            ViewBag.acc = HttpContext.Session.GetString("Account"); ;
            return View();
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(CLoginViewModels vm)
        {

            TestStaff x = _context.TestStaffs.FirstOrDefault(T => T.員工編號.Equals(vm.txtAccount) && T.密碼.Equals(vm.txtPassword));
            if (x != null)
            {
                if (x.密碼.Equals(vm.txtPassword) && x.員工編號.Equals(vm.txtAccount))
                {
                    string json = "";
                    List<CLoginViewModels> Account = null;
                    
                    if (HttpContext.Session.Keys.Contains("Account"))
                    {
                        json = HttpContext.Session.GetString("Account");
                        Account = System.Text.Json.JsonSerializer.Deserialize<List<CLoginViewModels>>(json);
                    }
                    
                    HttpContext.Session.SetString("Account", vm.txtAccount);

                    
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}
