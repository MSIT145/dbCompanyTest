using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace dbCompanyTest.Controllers
{
    public class MyLoveController : Controller
    {
        dbCompanyTestContext _context=new dbCompanyTestContext();
        List<CarViewModels> list = null;
        List<會員商品暫存> MyLovelist = null;
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {

                if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_MYLOVE_SESSION_NEW))
                {
                    string json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_MYLOVE_SESSION_NEW);
                    MyLovelist = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                    var data = _context.會員商品暫存s.Select(x => x).ToList().Where(y => y.購物車或我的最愛 != true && y.客戶編號.Contains("CL2-00667"));
                    MyLovelist.AddRange(data.ToList());
                    return View(MyLovelist);
                }
                else 
                { 
                var data = _context.會員商品暫存s.Select(x => x).ToList().Where(y => y.購物車或我的最愛 != true && y.客戶編號.Contains("CL2-00667"));
                return View(data.ToList());
                }
            }
            else 
            {
                return RedirectToAction(Url.Content("~/Login/Login"));
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
            else {
                return Content("沒有這筆資料");
            }
        }
        public IActionResult JoinCart() 
        {
            //if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION))
            //{
            //    string json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
            //    list = JsonSerializer.Deserialize<List<CarViewModels>>(json);
            //}
            //else 
            //{
            //    list = new List<CarViewModels>();

            
            
            //}
            return Content("成功加入購物車");
        }
    }
}
