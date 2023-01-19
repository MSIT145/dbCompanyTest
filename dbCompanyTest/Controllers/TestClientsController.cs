using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.ComponentModel;
using System.Data;
using System.Text.Json;

namespace dbCompanyTest.Controllers
{
    public class TestClientsController : Controller
    {
        private dbCompanyTestContext _context = new dbCompanyTestContext();
        private IWebHostEnvironment _eviroment; //宣告取域 環境變數
        private readonly ILogger<TestClientsController> _logger;  //設定成紀錄類型

        public TestClientsController(IWebHostEnvironment eviroment, ILogger<TestClientsController> logger)  //建構子，將環境變數注入
        {
            _eviroment = eviroment;
            _logger = logger; //注入，才能用session
        }
        //public TestClientsController(dbCompanyTestContext context)
        //{
        //    _context = context;
        //}

        // GET: TestClients
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestClients.ToListAsync());
        }

        public IActionResult Search(string keyPoint)
        {
            List<TestClient> clients = _context.TestClients.Where(x => x.客戶姓名.Contains(keyPoint)).ToList();
            if (clients.Count() == 0)
                return Json("沒有資料");
            else
                return Json(clients);
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

        public IActionResult print()
        {
            //把 Session 到回ListData
            List<TestClient> searchData = _context.TestClients.ToList();
            //string json;
            //if (HttpContext.Session.Keys.Contains(Product_CDictionary.SK_SEARCH_PRODUCTS_LIST)) //判斷Session 的key是否含有SK_PURCHASED_PRODUCTS_LIST
            //{
            //    json = HttpContext.Session.GetString(Product_CDictionary.SK_SEARCH_PRODUCTS_LIST);   //將Session 轉為字串
            //    searchData = JsonSerializer.Deserialize<List<TestClient>>(json);
            //}
            //else
            //    searchData = 


            if (searchData.Count == 0)
                return Json("沒有可輸出資料!!");

            //取得欄位名稱
            DataTable dt = ConvertToDataTable(searchData.ToArray());
            DataTableToExcelFile(dt);

            return Json("成功!!");
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        private string DataTableToExcelFile(DataTable dt)
        {
            //建立Excel 2003檔案
            IWorkbook wb = new HSSFWorkbook();
            ISheet ws= wb.CreateSheet("客戶資料");

            ////建立Excel 2007檔案
            //IWorkbook wb = new XSSFWorkbook();
            //ISheet ws;


            ws.CreateRow(0);//第一行為欄位名稱
            for (int i = 0; i < dt.Columns.Count-3; i++)
            {
                ws.GetRow(0).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ws.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count-3; j++)
                {
                    ws.GetRow(i + 1).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            string filename = "Client" + DateTime.Now.ToString("yyyyMMdd");
            string path = _eviroment.WebRootPath + "/Datas/" + $"{filename}.xls";  //用環境變數取得 IIS路徑(wwwroot)
            System.IO.File.Delete(path);   //刪掉原本的檔案           
            FileStream file = new FileStream(path, FileMode.Create);//產生檔案
            wb.Write(file);
            file.Close();
            return path;
        }
    }
}
