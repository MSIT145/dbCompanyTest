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

        public IActionResult memberInfo(string id)
        {
            id = "CL1-00376";
            TestClient client = _context.TestClients.FirstOrDefault(x => x.客戶編號 == id);
            if(client != null)
            {
                return View(client);
            }
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
