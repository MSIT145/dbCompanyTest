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
        List<會員商品暫存> shoppingcart = null;
        List<會員商品暫存> MyLovelist = null;
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                string login = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
                var user = JsonSerializer.Deserialize<TestClient>(login);
                if (!HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_MYLOVE_SESSION_OLD))
                {
                    var data = _context.會員商品暫存s.Select(x => x).Where(y => y.購物車或我的最愛 != true && y.客戶編號.Contains(user.客戶編號)).ToList();
                    string myloveold = JsonSerializer.Serialize(data);
                    HttpContext.Session.SetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION_OLD, myloveold);
                    return View(data);
                }
                else 
                {
                    var json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION_OLD);
                    var data = JsonSerializer.Deserialize<List < 會員商品暫存 >> (json);
                    if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_MYLOVE_SESSION_NEW))
                    {
                        string jsonnew = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION_NEW);
                        MyLovelist = JsonSerializer.Deserialize<List<會員商品暫存>>(jsonnew);
                        MyLovelist.AddRange(data);
                        return View(MyLovelist);
                    }
                    else
                    {
                        return View(data);
                    }
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
                var data = _context.會員商品暫存s.FirstOrDefault(x => x.Id == id);
                _context.會員商品暫存s.Remove(data);
                _context.SaveChanges();
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
                //讀出購物車session
                string cartjson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                shoppingcart = JsonSerializer.Deserialize<List<會員商品暫存>>(cartjson);
                //讀出我的最愛session
                string lovejson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION_NEW);
                MyLovelist = JsonSerializer.Deserialize<List<會員商品暫存>>(lovejson);
                會員商品暫存 data = new 會員商品暫存();
                data = MyLovelist.Find(x => x.Id == id);
                shoppingcart.Add(data);
                cartjson = JsonSerializer.Serialize(shoppingcart);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, cartjson);
                return Content("加入購物車成功");
            }
            else
            {
                string lovejson = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION_NEW);
                MyLovelist = JsonSerializer.Deserialize<List<會員商品暫存>>(lovejson);
                會員商品暫存 data = new 會員商品暫存();
                data = MyLovelist.Find(x => x.Id == id);
                shoppingcart.Add(data);
                string cartjson = JsonSerializer.Serialize(shoppingcart);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, cartjson);
                return Content("加入購物車成功");
            }
        }
    }
}
