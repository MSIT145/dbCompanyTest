using dbCompanyTest.Hubs;
using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace dbCompanyTest.Controllers
{
    public class MyLoveController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        public IActionResult Index()
        {
            //判斷是否登入
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                //取得Login Session
                string login = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
                var user = JsonSerializer.Deserialize<TestClient>(login);
                //判斷之前是否有Session
                if (!HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_MYLOVE_SESSION))
                {
                    //開啟MyloveSession
                    var data = _context.會員商品暫存s.Select(x => x).Where(y => y.購物車或我的最愛 == false && y.客戶編號.Contains(user.客戶編號)).ToList();
                    if (data.Count != 0)
                    {
                        string json = JsonSerializer.Serialize(data);
                        HttpContext.Session.SetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION, json);
                        return View(data);
                    }
                    return View(data);
                }
                else
                {
                    var json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                    var data = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                    return View(data);
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult Create(int id, string size,string color)
        {

            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                string userjson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
                var userinfo = JsonSerializer.Deserialize<TestClient>(userjson);
                //判斷之前是否有Session
                if (!HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_MYLOVE_SESSION))
                {
                    List<會員商品暫存> list = new List<會員商品暫存>();
                    會員商品暫存 data = new 會員商品暫存();
                    var dd = from item in _context.ProductDetails
                             join product in _context.Products on item.商品編號id equals product.商品編號id
                             join procolor in _context.ProductsColorDetails on item.商品顏色id equals procolor.商品顏色id
                             join prosize in _context.ProductsSizeDetails on item.商品尺寸id equals prosize.商品尺寸id
                             join prophoto in _context.圖片位置s on item.圖片位置id equals prophoto.圖片位置id
                             where item.Id == id
                                select new {
                                username = userinfo.客戶編號,
                                productID= item.商品編號id,
                                productName = product.商品名稱,
                                productPrice = product.商品價格,
                                productPhoto = prophoto.商品圖片1
                                };
                    foreach (var item in dd) {
                        data.商品價格 = item.productPrice;
                        data.商品名稱 = item.productName;
                        data.商品編號 = item.productID;
                        data.商品顏色種類 = color;
                        data.圖片1檔名 = item.productPhoto.ToString();
                        data.客戶編號 = item.username;
                        data.尺寸種類 = size;
                        data.購物車或我的最愛 = false;
                        data.訂單數量 = 1;
                    }
                    list.Add(data);
                    string json = JsonSerializer.Serialize(list);
                    HttpContext.Session.SetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION, json);
                    return Content("已加入收藏");

                }
                else
                {
                    var json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                    var list = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                    會員商品暫存 data = new 會員商品暫存();
                    var dd = from item in _context.ProductDetails
                             join product in _context.Products on item.商品編號id equals product.商品編號id
                             join procolor in _context.ProductsColorDetails on item.商品顏色id equals procolor.商品顏色id
                             join prosize in _context.ProductsSizeDetails on item.商品尺寸id equals prosize.商品尺寸id
                             join prophoto in _context.圖片位置s on item.圖片位置id equals prophoto.圖片位置id
                             where item.Id == id
                             select new
                             {
                                 username = userinfo.客戶編號,
                                 productID = item.商品編號id,
                                 productName = product.商品名稱,
                                 productPrice = product.商品價格,
                                 productPhoto = prophoto.商品圖片1
                             };
                    foreach (var item in dd)
                    {
                        data.商品價格 = item.productPrice;
                        data.商品名稱 = item.productName;
                        data.商品編號 = item.productID;
                        data.商品顏色種類 = color;
                        data.圖片1檔名 = item.productPhoto;
                        data.客戶編號 = item.username;
                        data.尺寸種類 = size;
                        data.購物車或我的最愛 = false;
                        data.訂單數量 = 1;
                    }
                    list.Add(data);
                    json = JsonSerializer.Serialize(list);
                    HttpContext.Session.SetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION, json);
                    return Content("已加入收藏");
                }
            }
            else
            {
                return Content("請先登入會員");
            }
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                string json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                var data = datas.FirstOrDefault(x => x.Id == id);
                datas.Remove(data);
                json = JsonSerializer.Serialize(datas);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION, json);
                return Content("刪除成功");
            }
            else
            {
                return Content("沒有這筆資料");
            }
        }
        public IActionResult JoinCart(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION))
            {
                List<會員商品暫存> shoppingcart = new List<會員商品暫存>();
                //讀出購物車session
                string cartjson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                shoppingcart = JsonSerializer.Deserialize<List<會員商品暫存>>(cartjson);
                //讀出我的最愛session
                string lovejson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(lovejson);
                會員商品暫存 data = null;
                data = datas.FirstOrDefault(x => x.Id == id);
                data.購物車或我的最愛 = true;
                shoppingcart.Add(data);
                cartjson = JsonSerializer.Serialize(shoppingcart);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, cartjson);
                return Content("加入購物車成功");
            }
            else
            {
                List<會員商品暫存> shoppingcart = new List<會員商品暫存>();
                string lovejson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(lovejson);
                會員商品暫存 data = null;
                data = datas.FirstOrDefault(x => x.Id == id);
                data.購物車或我的最愛 = true;
                shoppingcart.Add(data);
                string cartjson = JsonSerializer.Serialize(shoppingcart);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, cartjson);
                return Content("加入購物車成功");
            }
        }
    }
}
