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
            var data = from c in _context.Orders
                       join d in _context.OrderDetails on c.訂單編號 equals d.訂單編號
                       join e in _context.ProductDetails on d.Id equals e.Id
                       join f in _context.ProductsColorDetails on e.商品顏色id equals f.商品顏色id
                       join g in _context.ProductsSizeDetails on e.商品尺寸id equals g.商品尺寸id
                       join h in _context.圖片位置s on e.圖片位置id equals h.圖片位置id
                       join i in _context.Products on e.商品編號id equals i.商品編號id
                       where c.訂單編號 == id
                       select new ViewModels.MemberCenterViewModel
                       {
                           訂單編號=d.訂單編號,
                           商品名 = i.商品名稱,
                           圖片=h.商品圖片1,
                           規格=f.商品顏色種類+" / "+g.尺寸種類,
                           數量=d.商品數量,
                           價格=d.商品價格,
                           付款方式=c.付款方式,
                           送貨地址=c.送貨地址,
                           總金額=c.總金額
                       };

            return View(data);
        }

    }
}
