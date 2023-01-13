using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dbCompanyTest.Models;

namespace dbCompanyTest.Controllers
{
    public class TestClientsController : Controller
    {
        private dbCompanyTestContext _context = new dbCompanyTestContext();

        //public TestClientsController(dbCompanyTestContext context)
        //{
        //    _context = context;
        //}

        // GET: TestClients
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestClients.ToListAsync());
        }

        // GET: TestClients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TestClients == null)
            {
                return NotFound();
            }

            var testClient = await _context.TestClients
                .FirstOrDefaultAsync(m => m.客戶編號 == id);
            if (testClient == null)
            {
                return NotFound();
            }

            return View(testClient);
        }

        // GET: TestClients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestClients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("客戶編號,客戶姓名,客戶電話,身分證字號,縣市,區,地址,Email,密碼,性別,生日")] TestClient testClient)
        {
            if (!_context.TestClients.Any(c => c.Email == testClient.Email || c.客戶電話 == testClient.客戶電話 || c.身分證字號 == testClient.身分證字號))
            {
                var count = _context.TestClients.Count() + 1;
                testClient.客戶編號 = $"CL{testClient.身分證字號.Substring(1, 1)}-{count.ToString("0000")}{testClient.身分證字號.Substring(7, 3)}";
                _context.Add(testClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testClient);
        }

        // GET: TestClients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TestClients == null)
            {
                return NotFound();
            }

            var testClient = await _context.TestClients.FindAsync(id);
            if (testClient == null)
            {
                return NotFound();
            }
            return View(testClient);
        }

        // POST: TestClients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("客戶編號,客戶姓名,客戶電話,身分證字號,縣市,區,地址,Email,密碼,性別,生日")] TestClient testClient)
        {
            if (id != testClient.客戶編號)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestClientExists(testClient.客戶編號))
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
            return View(testClient);
        }

        // GET: TestClients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TestClients == null)
            {
                return NotFound();
            }

            var testClient = await _context.TestClients
                .FirstOrDefaultAsync(m => m.客戶編號 == id);
            if (testClient == null)
            {
                return NotFound();
            }

            return View(testClient);
        }

        // POST: TestClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TestClients == null)
            {
                return Problem("Entity set 'dbCompanyTestContext.TestClients'  is null.");
            }
            var testClient = await _context.TestClients.FindAsync(id);
            if (testClient != null)
            {
                _context.TestClients.Remove(testClient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestClientExists(string id)
        {
            return _context.TestClients.Any(e => e.客戶編號 == id);
        }

        public IActionResult checkJoindata(string EIP)
        {
            return Content(_context.TestClients.Any(e => e.Email == EIP || e.客戶電話 == EIP || e.身分證字號 == EIP).ToString());
        }
    }
}
