using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Security.Principal;
using static NuGet.Packaging.PackagingConstants;

namespace dbCompanyTest.Controllers
{
    public class Staff_HomeController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        public IActionResult Index()
        {
            string stfNum = HttpContext.Session.GetString("Account");
            var stf = _context.TestStaffs.FirstOrDefault(c => c.員工編號 == stfNum);

            ViewBag.acc = $"{stfNum} {stf.員工姓名} {stf.部門}";
            return View();
        }
        public async Task<IActionResult> Index_HR()
        {
            string stfNum = HttpContext.Session.GetString("Account");
            var stf = _context.TestStaffs.FirstOrDefault(c => c.員工編號 == stfNum);

            ViewBag.HR = $"{stfNum} {stf.員工姓名} {stf.部門}";
            return View(await _context.TestStaffs.ToListAsync());
        }

        public IActionResult login()
        {
            HttpContext.Session.Remove("Account");
            return View();
        }
        [HttpPost]
        public IActionResult login(CLoginViewModels vm)
        {
            TestStaff x = _context.TestStaffs.FirstOrDefault(T => T.員工編號.Equals(vm.txtAccount) && T.密碼.Equals(vm.txtPassword));
            if (x != null)
            {
                if (x.密碼.Equals(vm.txtPassword) && x.員工編號.Equals(vm.txtAccount))
                {
                    if (!HttpContext.Session.Keys.Contains("Account"))
                        HttpContext.Session.SetString("Account", vm.txtAccount);
                    if (x.部門 == "行政")
                    {
                        return RedirectToAction("Index");
                    }
                    else if(x.部門 == "人事")
                    {
                        return RedirectToAction("Index_HR");
                    }
                }

            }
            return View();
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("Account");
            return RedirectToAction("login");
        }
        public IActionResult PartialSheeplist()
        {
            return PartialView();
        }
        public IActionResult PartialToDoList()
        {
            return PartialView();
        }


        //==================================================
        public IActionResult LoadSheeplist()
        {
            var datas = from c in _context.Orders
                        join o in _context.OrderDetails on c.訂單編號 equals o.訂單編號
                        join a in _context.ProductDetails on o.Id equals a.Id
                        join b in _context.ProductsSizeDetails on a.商品尺寸id equals b.商品尺寸id
                        join d in _context.ProductsColorDetails on a.商品顏色id equals d.商品顏色id
                        join e in _context.Products on a.商品編號id equals e.商品編號id
                        where c.訂單狀態 == "待出貨"
                        select new ViewModels.Cback_order_list
                        {
                            訂單編號 = c.訂單編號,
                            客戶編號 = c.客戶編號,
                            送貨地址 = c.送貨地址,
                            商品名稱 = e.商品名稱,
                            尺寸種類 = b.尺寸種類,
                            色碼 = d.色碼,
                            商品數量 = (int)o.商品數量
                        };
            var test = datas.ToList();
            return Json(datas);
        }

        public IActionResult LoadToDoList(string stf)
        {
            var datas = from c in _context.ToDoLists
                        join o in _context.TestStaffs on c.員工編號 equals o.員工編號
                        where c.員工編號 == stf
                        select c;

            return Json(datas);
        }
    }
}
