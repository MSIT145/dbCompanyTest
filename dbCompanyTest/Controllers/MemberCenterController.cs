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
            
            TestClient client = _context.TestClients.FirstOrDefault(x => x.客戶編號 == id);
            if(client != null)
            {
                return View(client);
            }
            return View();
        }

        public IActionResult orderInfo(string id)
        {
            var data = from c in _context.Orders
                       where c.客戶編號 ==id
                       select c;
            return View(data);
        }

        public IActionResult orderInfoDetail(string id)
        {

            return View();
        }

    }
}
