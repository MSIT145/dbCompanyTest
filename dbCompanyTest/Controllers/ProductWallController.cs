﻿using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
                            join e in _context.ProductsTypeDetails on d.商品分類id equals e.商品分類id
                            where e.商品分類id == id
                            select c;
            return View(datas);

            }
        }






        //---------------------- Gary產品頁 ----------------------------
        public IActionResult Details(int? id) 
        {
            List<ProductDetailViewModels> PDMS = new List<ProductDetailViewModels>();
            ProductDetailViewModels pdm = null;
            if (id == null)
                return NotFound();
            else
            {
<<<<<<< Updated upstream
                var productdetail = _context.ProductDetails.FirstOrDefault(x => x.商品編號id == id);
                string productname = _context.Products.FirstOrDefault(x => x.商品編號id == id).商品名稱.ToString();
                
                ViewBag.Productname = productname;
                var product商品分類 = from c in _context.Products
                                  join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
                                  join e in _context.ProductsTypeDetails on d.商品分類id equals e.商品分類id
                                  select e.商品分類名稱.ToString();
                ViewBag.商品分類 = product商品分類;
                return View(productdetail);
=======
                var productdetail = from c in _countext.Products
                                    join prod in _countext.ProductDetails on c.商品編號id equals prod.商品編號id
                                    join procolor in _countext.ProductsColorDetails on prod.商品顏色id equals procolor.商品顏色id
                                    join prosize in _countext.ProductsSizeDetails on prod.商品尺寸id equals prosize.商品尺寸id
                                    join pro分類 in _countext.ProductsTypeDetails on prod.商品分類id equals pro分類.商品分類id
                                    where c.商品編號id ==id
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
>>>>>>> Stashed changes
            }
            
        }
    }
}
