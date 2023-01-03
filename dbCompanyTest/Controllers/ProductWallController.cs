using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ProductWallController : Controller
    {
        dbCompanyTestContext _countext = new dbCompanyTestContext();
        public IActionResult Index()
        {
            var datas = from c in _countext.Products
                        select c;
            return View(datas);
        }






        //---------------------- Gary產品頁 ----------------------------
        public IActionResult Details(int? id) 
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
