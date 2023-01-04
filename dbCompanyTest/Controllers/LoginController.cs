using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModels x)
        {
            dbCompanyTestContext db = new dbCompanyTestContext();
            if (db.TestClients.Where(c => c.Email == x.txtAccount && c.密碼 == x.txtPassword) != null)
            {
                return RedirectToAction();
            }
            else
            {
                x.txtPassword = "";
                return View(x);
            }
        }

        public IActionResult loginSussess()
        {
            return View();
        }
    }
}
