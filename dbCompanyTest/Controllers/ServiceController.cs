using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Service()
        {
            return View();
        }
    }
}
