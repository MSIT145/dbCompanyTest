using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.Xml;

namespace dbCompanyTest.Controllers
{
    public class ProductWallController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();

        public IActionResult Index(int? id)
        {
            if (id == null)
                return NotFound();
            else
            {


                var datas = from c in _context.Products
                            join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
                            join f in _context.圖片位置s on d.圖片位置id equals f.圖片位置id
                            join b in _context.商品鞋種s on c.商品鞋種id equals b.商品鞋種id
                            where c.商品分類id == id
                            select new ViewModels.ProductWallViewModel
                            {
                                鞋種名稱 = b.鞋種,
                                商品編號id = c.商品編號id,
                                商品名稱 = c.商品名稱,
                                商品價格 = (decimal)c.商品價格,
                                產品圖片1 = f.商品圖片1
                            };

                return View(datas.ToList());






            }
        }
 //---------------------- Gary產品頁 ----------------------------
            public IActionResult Details(int? id)
            {
                id = 1;
                List<ProductDetailViewModels> PDMS = new List<ProductDetailViewModels>();
                ProductDetailViewModels pdm = null;
                if (id == null)
                    return NotFound();
                else
                {
                    var productdetail = from c in _context.Products
                                        join prod in _context.ProductDetails on c.商品編號id equals prod.商品編號id
                                        join procolor in _context.ProductsColorDetails on prod.商品顏色id equals procolor.商品顏色id
                                        join prosize in _context.ProductsSizeDetails on prod.商品尺寸id equals prosize.商品尺寸id
                                        join pro分類 in _context.ProductsTypeDetails on c.商品分類id equals pro分類.商品分類id

                                        where c.商品編號id == id
                                        select new
                                        {
                                            pro商品id = c.商品編號id,
                                            pro商品名稱 = c.商品名稱,
                                            pro商品色碼 = procolor.色碼,
                                            pro商品顏色 = procolor.商品顏色種類,
                                            pro商品尺寸 = prosize.尺寸種類,
                                            pro商品分類 = pro分類.商品分類名稱
                                        };
                    foreach (var item in productdetail)
                    {
                        pdm = new ProductDetailViewModels();
                        pdm.pro商品id = item.pro商品id;
                        pdm.pro商品顏色 = item.pro商品顏色;
                        pdm.pro商品尺寸 = item.pro商品尺寸;
                        pdm.pro商品分類 = item.pro商品分類;
                        pdm.pro商品色碼 = item.pro商品色碼;
                        pdm.pro商品名稱 = item.pro商品名稱;
                        PDMS.Add(pdm);
                    }
                    return View(PDMS);

                }

            }
        }
    }
