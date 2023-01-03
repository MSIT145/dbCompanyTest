using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ProductWallController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }






        //---------------------- Gary產品頁 ----------------------------
        public IActionResult Detail(int? id) 
        {
            if(id == null)
                return NotFound();
            return View(id);
        }
    }
}
