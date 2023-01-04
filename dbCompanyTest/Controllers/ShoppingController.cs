﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dbCompanyTest.Models;

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
            List<會員商品暫存> list=new List<會員商品暫存>();
            //var dbCompanyTestContext = _context.會員商品暫存s.Include(會 => 會.客戶編號Navigation).Where(c => c.購物車或我的最愛==true && c.客戶編號.Contains(name)).ToListAsync();
            //foreach(var c in dbCompanyTestContext.l)
            var datas = from c in _context.會員商品暫存s where c.購物車或我的最愛 ==true && c.客戶編號==name
                        //join o in 
                        select c;
            return View(datas);
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
        public IActionResult Create()
        {
            ViewData["客戶編號"] = new SelectList(_context.TestClients, "客戶編號", "客戶編號");
            return View();
        }

        // POST: Shopping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,客戶編號,商品編號,商品名稱,尺寸種類,商品顏色種類,訂單數量,商品價格,購物車或我的最愛")] 會員商品暫存 會員商品暫存)
        {
            if (ModelState.IsValid)
            {
                _context.Add(會員商品暫存);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["客戶編號"] = new SelectList(_context.TestClients, "客戶編號", "客戶編號", 會員商品暫存.客戶編號);
            return View(會員商品暫存);
        }

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
