using dbCompanyTest.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dbCompanyTest.Controllers
{
    public class HomeController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            var data = from c in _context.ProductsTypeDetails
                       //join d in _context.Products on c.商品分類id equals d.商品分類id
                       //join e in _context.ProductDetails on d.商品編號id equals e.商品編號id
                       //join f in _context.OrderDetails on e.Id equals f.Id
                       select c;
            return View();
        }

        public IActionResult nav()
        {
            var datas = from c in _context.ProductsTypeDetails
                        select c;
            return PartialView(datas);
        }

   
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}