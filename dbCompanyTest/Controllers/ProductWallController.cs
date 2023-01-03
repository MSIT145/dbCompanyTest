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
            if (id == null)
                return NotFound();
            else
            {
                var productdetail = _countext.ProductDetails.FirstOrDefault(x => x.商品編號id == id);
                string productname = _countext.Products.FirstOrDefault(x => x.商品編號id == id).商品名稱.ToString();
                ViewBag.Productname = productname;
                return View(productdetail);
            }
            
        }
    }
}
