using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class MemberCenterController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult memberInfo(string? email)
        {
            var data = from c in _context.TestClients
                       where c.Email == email
                       select c;
            return View();
        }

        public IActionResult orderInfo()
        {

            return View();
        }

        public IActionResult orderInfoDetail()
        {

            return View();
        }

    }
}
