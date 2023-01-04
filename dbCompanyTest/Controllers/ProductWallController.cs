using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace dbCompanyTest.Controllers
{
    public class ProductWallController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        public IActionResult Index(int? id)
        {
            if (id == null)
                return NotFound();
            else
            {
                var datas = from c in _context.Products
                            join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
                            join e in _context.ProductsTypeDetails on d.商品分類id equals e.商品分類id
                            where e.商品分類id == id
                            select c;
            return View(datas);

            }
        }






        //---------------------- Gary產品頁 ----------------------------
        public IActionResult Details(int? id) 
        {
            if (id == null)
                return NotFound();
            else
            {
                var productdetail = _context.ProductDetails.FirstOrDefault(x => x.商品編號id == id);
                string productname = _context.Products.FirstOrDefault(x => x.商品編號id == id).商品名稱.ToString();
                
                ViewBag.Productname = productname;
                var product商品分類 = from c in _context.Products
                                  join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
                                  join e in _context.ProductsTypeDetails on d.商品分類id equals e.商品分類id
                                  select e.商品分類名稱.ToString();
                ViewBag.商品分類 = product商品分類;
                return View(productdetail);
            }
            
        }
    }
}
