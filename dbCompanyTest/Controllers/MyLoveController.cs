using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text;
using System.Xml.Linq;

namespace dbCompanyTest.Controllers
{
    public class MyLoveController : Controller
    {
        dbCompanyTestContext _context=new dbCompanyTestContext();
        public IActionResult Index()
        {
            var data = _context.會員商品暫存s.Select(x=>x).ToList().Where(y=>y.購物車或我的最愛!=true&&y.客戶編號.Contains("CL2-00667"));
            return View(data);
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
        public IActionResult JoinCart(string proname,string procolor,string prosize,int count) 
        {

            return Content("成功加入購物車"+proname+procolor+prosize+count);
        }
    }
}
