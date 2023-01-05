using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class MyLoveController : Controller
    {
        dbCompanyTestContext _context=new dbCompanyTestContext();
        public IActionResult Index()
        {
            var data = _context.會員商品暫存s.Select(x=>x).ToList().Where(y=>y.購物車或我的最愛!=true&&y.客戶編號.Contains("CL1-00376"));
            return View(data);
        }
    }
}
