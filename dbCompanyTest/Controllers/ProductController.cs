using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ProductController : Controller
    {
        dbCompanyTestContext db = new dbCompanyTestContext();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult showlist()
        {
            var data =from  p in db.Products.ToList()
                      join pt in db.ProductsTypeDetails on p.商品分類id equals pt.商品分類id
                      join s in db.商品鞋種s on p.商品鞋種id equals s.商品鞋種id
                      select new BACK_ProductViewModels 
                      {
                            商品編號id  = p.商品編號id,
                            上架時間 =p.上架時間,
                            商品名稱 = p.商品名稱,
                            商品價格 = p.商品價格,
                            商品介紹 = p.商品介紹,
                            商品材質  = p.商品材質,
                            商品重量  = p.商品重量,
                            商品成本 = p.商品成本,
                            商品是否有貨 = p.商品是否有貨,
                            商品分類  = pt.商品分類名稱,
                            商品鞋種 = s.鞋種,
                            商品是否上架  = p.商品是否上架
                       };

            return Json(new { data });
        }

        [HttpPost]
        public IActionResult showDetail(string id) 
        { 
            if(string.IsNullOrEmpty(id))
            Convert.ToInt32(id);
           return View();
        }
    }
}
