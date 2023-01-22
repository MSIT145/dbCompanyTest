using dbCompanyTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ProductRelevantController : Controller
    {
        dbCompanyTestContext db = new dbCompanyTestContext();
        private IWebHostEnvironment _eviroment; //宣告取域 環境變數
        private readonly ILogger<ProductController> _logger;  //設定成紀錄類型

        public ProductRelevantController(IWebHostEnvironment eviroment, ILogger<ProductController> logger)  //建構子，將環境變數注入
        {
            _eviroment = eviroment;
            _logger = logger; //注入，才能用session
        }
        //存放Product的資料，搜尋請改用Session
        //static List<Product> searchData = new List<Product>();

        public IActionResult Index()
        {
            return View();
        }
    }
}
