using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ProductWallController : Controller
    {
        dbCompanyTestContext _countext = new dbCompanyTestContext();
        public IActionResult Index()
        {
            return View();
        }






        //---------------------- Gary產品頁 ----------------------------
        public IActionResult Detail(int? id) 
        {
            id = 1;
            if (id == null)
                return NotFound();
            else
            {
                var productdetail = from item in _countext.ProductDetails
                                    where item.Id == id
                                    select item;
                return View(productdetail);
            }
            
        }
    }
}
