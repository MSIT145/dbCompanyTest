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
                    var data = _context.會員商品暫存s.Select(x => x).Where(y => y.購物車或我的最愛 != true && y.客戶編號.Contains(user.客戶編號)).ToList();
                    string json = JsonSerializer.Serialize(data);
                    HttpContext.Session.SetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION, json);
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
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                string json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                var data = datas.FirstOrDefault(x => x.Id == id);
                datas.Remove(data);
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
                List<會員商品暫存> shoppingcart = null;
                //讀出購物車session
                string cartjson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                shoppingcart = JsonSerializer.Deserialize<List<會員商品暫存>>(cartjson);
                //讀出我的最愛session
                string lovejson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(lovejson);
                會員商品暫存 data = new 會員商品暫存();
                data = datas.Find(x => x.Id == id);
                shoppingcart.Add(data);
                cartjson = JsonSerializer.Serialize(shoppingcart);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, cartjson);
                return Content("加入購物車成功");
            }
            else
            {
                List<會員商品暫存> shoppingcart = null;
                string lovejson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(lovejson);
                會員商品暫存 data = new 會員商品暫存();
                data = datas.Find(x => x.Id == id);
                shoppingcart.Add(data);
                string cartjson = JsonSerializer.Serialize(shoppingcart);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, cartjson);
                return Content("加入購物車成功");
            }
        }
    }
}
