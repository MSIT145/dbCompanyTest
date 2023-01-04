using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

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
                var product商品分類 = from c in _countext.Products
                                  join d in _countext.ProductDetails on c.商品編號id equals d.商品編號id
                                  join e in _countext.ProductsTypeDetails on d.商品分類id equals e.商品分類id
                                  select e.商品分類名稱.ToString();
                ViewBag.商品分類 = product商品分類;
                return View(productdetail);
            }
            
        }
    }
}
