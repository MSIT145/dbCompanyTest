using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public void loginSussess()
        {

        }
    }
}
