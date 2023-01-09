using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class Staff_HomeController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        public IActionResult Index()
        {
            string acc = TempData["Account"] as string;
            ViewBag.acc = acc;
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
                    if (HttpContext.Session.Keys.Contains("Account"))
                        Account = HttpContext.Session.GetString("Account");
                    
                    HttpContext.Session.SetString("Account", vm.txtAccount);

                    TempData["Account"] = Account;  

                    
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}
