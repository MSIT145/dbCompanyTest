using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using System.Text.Json;
using System.Text;
using System.Data;

namespace dbCompanyTest.Controllers
{
    public class ShoppingController : Controller
    {
        dbCompanyTestContext _context=new dbCompanyTestContext();
        string name = "CL1-0005891";
        //private readonly dbCompanyTestContext _context;

        //public ShoppingController(dbCompanyTestContext context)
        //{
        //    _context = context;
        //}

        // GET: Shopping
        public  IActionResult Index()
        {
            //List<CarViewModels> list=null;

            var dbCompanyTestContext = _context.會員商品暫存s.Include(會 => 會.客戶編號Navigation).Where(c => c.購物車或我的最愛 == true && c.客戶編號.Contains(name)).ToList();
            //foreach (var c in dbCompanyTestContext.l)

            //    var datas = from c in _context.會員商品暫存s where c.購物車或我的最愛 ==true && c.客戶編號==name
            //            join o in _context.ProductDetails on c.商品編號 equals o.商品編號id
            //            join a in _context.圖片位置s on o.圖片位置id equals a.圖片位置id
            //           select new
            //           {
            //               a.商品圖片1,
            //               c.商品名稱,
            //               c.商品價格,
            //               c.商品顏色種類,
            //               c.尺寸種類,
            //               c.訂單數量,
            //               c.Id,
            //               c.客戶編號,
            //               c.商品編號
            //           };
            //if (datas == null)
            //    return Redirect("../Home/Index");
            //string json = "";
            //if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION))
            //{
            //    json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
            //    list = JsonSerializer.Deserialize<List<CarViewModels>>(json);
            //}
            //else
            //    list = new List<CarViewModels>();

            //foreach (var data in dbCompanyTestContext)
            //{
            //    CarViewModels c = new CarViewModels();
            //    if (data.商品圖片1 == "0201")
            //        c.car圖片位置.商品圖片1 = "404.jpg";
            //    else
            //        c.car圖片位置.商品圖片1 = data.商品圖片1;
            //    c.訂單數量 = data.訂單數量;
            //    c.商品顏色種類 = data.商品顏色種類;
            //    c.商品編號 = data.商品編號;
            //    c.Id=data.Id;
            //    c.商品名稱 = data.商品名稱;
            //    c.商品價格 = data.商品價格;
            //    c.客戶編號 = data.客戶編號;
            //    c.尺寸種類=data.尺寸種類;
            //    list.Add(c);
                //json = JsonSerializer.Serialize(dbCompanyTestContext);
                //HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, json);
            //}

            //var datas = (_context.會員商品暫存s).Join(_context.ProductDetails, c => c.商品編號, o => o.商品編號id);
            return View(dbCompanyTestContext);
        }

        public IActionResult GetDeliveryMony(string OPvalue)
        {
            if (string.IsNullOrEmpty(OPvalue))
                OPvalue = "0";

            return Content($"{OPvalue}", "text/plain", Encoding.UTF8);
        }



        public IActionResult CarPartial()
        {
            return PartialView(_context.會員商品暫存s.Include(會 => 會.客戶編號Navigation).Where(c => c.購物車或我的最愛 == true && c.客戶編號.Contains(name)));
        }
        public IActionResult CarTwdPartial()
        {
            return PartialView(_context.會員商品暫存s.Include(會 => 會.客戶編號Navigation).Where(c => c.購物車或我的最愛 == true && c.客戶編號.Contains(name)));
        }






        // GET: Shopping/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.會員商品暫存s == null)
            {
                return NotFound();
            }

            var 會員商品暫存 = await _context.會員商品暫存s
                .Include(會 => 會.客戶編號Navigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (會員商品暫存 == null)
            {
                return NotFound();
            }

            return View(會員商品暫存);
        }

        // GET: Shopping/Create
        //public IActionResult Create()
        //{
        //    ViewData["客戶編號"] = new SelectList(_context.TestClients, "客戶編號", "客戶編號");
        //    return View();
        //}

        // POST: Shopping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,客戶編號,商品編號,商品名稱,尺寸種類,商品顏色種類,訂單數量,商品價格,購物車或我的最愛")] 會員商品暫存 會員商品暫存)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(會員商品暫存);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["客戶編號"] = new SelectList(_context.TestClients, "客戶編號", "客戶編號", 會員商品暫存.客戶編號);
        //    return View(會員商品暫存);
        //}

        // GET: Shopping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.會員商品暫存s == null)
            {
                return NotFound();
            }

            var 會員商品暫存 = await _context.會員商品暫存s.FindAsync(id);
            if (會員商品暫存 == null)
            {
                return NotFound();
            }
            ViewData["客戶編號"] = new SelectList(_context.TestClients, "客戶編號", "客戶編號", 會員商品暫存.客戶編號);
            return View(會員商品暫存);
        }

        // POST: Shopping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,客戶編號,商品編號,商品名稱,尺寸種類,商品顏色種類,訂單數量,商品價格,購物車或我的最愛")] 會員商品暫存 會員商品暫存)
        {
            if (id != 會員商品暫存.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(會員商品暫存);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!會員商品暫存Exists(會員商品暫存.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["客戶編號"] = new SelectList(_context.TestClients, "客戶編號", "客戶編號", 會員商品暫存.客戶編號);
            return View(會員商品暫存);
        }

        // GET: Shopping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.會員商品暫存s == null)
            {
                return NotFound();
            }

            var 會員商品暫存 = await _context.會員商品暫存s
                .Include(會 => 會.客戶編號Navigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (會員商品暫存 == null)
            {
                return NotFound();
            }

            return View(會員商品暫存);
        }

        // POST: Shopping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.會員商品暫存s == null)
            {
                return Problem("Entity set 'dbCompanyTestContext.會員商品暫存s'  is null.");
            }
            var 會員商品暫存 = await _context.會員商品暫存s.FindAsync(id);
            if (會員商品暫存 != null)
            {
                _context.會員商品暫存s.Remove(會員商品暫存);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool 會員商品暫存Exists(int id)
        {
          return _context.會員商品暫存s.Any(e => e.Id == id);
        }
    }
}
