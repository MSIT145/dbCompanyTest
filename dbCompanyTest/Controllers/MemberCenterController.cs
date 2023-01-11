using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
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
                ViewBag.city = client.縣市;
                ViewBag.area = client.區;
                ViewBag.sex = client.性別;
                return View(client);
            }
            return View();
        }
        [HttpPost]
        public IActionResult memberInfo(MemberCenterViewModel vm)
        {
            TestClient client = _context.TestClients.FirstOrDefault(x => x.客戶編號 == vm.客戶編號);
            if (client != null)
            {
                client.客戶姓名 = vm.客戶姓名;
                client.客戶電話 = vm.客戶電話;
                client.身分證字號 = vm.身分證字號;
                client.縣市 = vm.縣市;
                client.區 = vm.區;
                client.地址 = vm.地址;
                client.Email = vm.Email;
                client.性別 = vm.性別;
                client.生日 = vm.生日;
                
                _context.SaveChanges();
                
            }
            return View();
        }


        public IActionResult password(MemberCenterViewModel vm)
        {
            TestClient client = _context.TestClients.FirstOrDefault(x => x.客戶編號 == vm.客戶編號) ;
            if (client != null)
            {
                client.密碼 = vm.密碼;

                _context.SaveChanges();
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
                       select new ViewModels.OrderInfoDetailViewModel
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
                        
            return View(data.ToList());
        }

    }
}
