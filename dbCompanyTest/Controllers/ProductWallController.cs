using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography.Xml;
using X.PagedList;


namespace dbCompanyTest.Controllers
{
    public class ProductWallController : Controller
    {
        dbCompanyTestContext _context = new dbCompanyTestContext();

        public IActionResult Index(int? id, int page = 1)
        {

    

            if (id == null)
                return NotFound();
            else
            {
                var datas = from c in _context.Products
                            join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
                            join e in _context.ProductsTypeDetails on c.商品分類id equals e.商品分類id
                            join f in _context.圖片位置s on d.圖片位置id equals f.圖片位置id
                            join b in _context.商品鞋種s on c.商品鞋種id equals b.商品鞋種id
                            where c.商品分類id == id
                            select new ViewModels.ProductWallViewModel
                            {
                                鞋種名稱 = b.鞋種,
                                商品id = d.Id,
                                商品分類id = (int)id,
                                商品鞋種id = (int)c.商品鞋種id,
                                商品名稱 = c.商品名稱,
                                商品價格 = (decimal)c.商品價格,
                                產品圖片1 = f.商品圖片1,
                                商品分類名稱 = e.商品分類名稱

                            };

                return View(datas.ToPagedList(page,5));
            }
        }

        public IActionResult type(int? id,int? tid,string? type, int page = 1)
        {
            if (id == null)
                return NotFound();
            else
            {
                var datas = from c in _context.Products
                            join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
                            join e in _context.ProductsTypeDetails on c.商品分類id equals e.商品分類id
                            join f in _context.圖片位置s on d.圖片位置id equals f.圖片位置id
                            join b in _context.商品鞋種s on c.商品鞋種id equals b.商品鞋種id
                            where c.商品鞋種id == id
                            select new ViewModels.ProductWallViewModel
                            {
                                鞋種名稱 = b.鞋種,
                                商品id = d.Id,
                                商品分類id = (int)tid,
                                商品鞋種id = (int)c.商品鞋種id,
                                商品名稱 = c.商品名稱,
                                商品價格 = (decimal)c.商品價格,
                                產品圖片1 = f.商品圖片1,
                                商品分類名稱 = type
                            };
                
                return View(datas.ToPagedList(page, 5));
            }
        }

        //public IActionResult shoesclass(int? id)
        //{
        //    id = 1;
        //    if (id == null)
        //        return NotFound();
        //    else
        //    {
        //        var datas = from c in _context.Products
        //                    join d in _context.ProductDetails on c.商品編號id equals d.商品編號id
        //                    join e in _context.ProductsTypeDetails on c.商品分類id equals e.商品分類id
        //                    join f in _context.圖片位置s on d.圖片位置id equals f.圖片位置id
        //                    join b in _context.商品鞋種s on c.商品鞋種id equals b.商品鞋種id
        //                    where c.商品分類id == id
        //                    select new ViewModels.ProductWallViewModel
        //                    {
        //                        鞋種名稱 = b.鞋種,
        //                        商品編號id = c.商品編號id,
        //                        商品鞋種id = (int)c.商品鞋種id,
        //                    };

        //        return PartialView(datas);
        //    }
        //}


        //---------------------- Gary產品頁 ----------------------------
        public IActionResult Details(int? id)
        {
            //測試用 productDetail ID
            if (id == null) 
            {
                id = 4;
            }
            ProductDetailViewModels pdm = new ProductDetailViewModels();
            
            pdm.pro商品顏色圖片list = new List<string>();
            pdm.pro商品尺寸list = new List<string>();
            pdm.pro商品DetailIDlist = new List<int>();
            int Key = 0;
            if (id == null)
                return NotFound();
            else
            {
                var productdetail = from prodetail in _context.ProductDetails
                                    join product in _context.Products on prodetail.商品編號id equals product.商品編號id
                                    join pro分類 in _context.ProductsTypeDetails on product.商品分類id equals pro分類.商品分類id
                                    join prophoto in _context.圖片位置s on prodetail.圖片位置id equals prophoto.圖片位置id
                                    join procolor in _context.ProductsColorDetails on prodetail.商品顏色id equals procolor.商品顏色id
                                    where prodetail.Id == id
                                    select new
                                    {
                                        prodetailID = prodetail.Id,
                                        pro商品編號 = prodetail.商品編號id,
                                        pro商品金額 = product.商品價格,
                                        pro商品顏色 = procolor.商品顏色種類,
                                        pro商品顏色圖片 = procolor.商品顏色圖片,
                                        pro商品分類 = pro分類.商品分類名稱,
                                        pro商品名稱 = product.商品名稱,
                                        pro商品介紹 = product.商品介紹,
                                        pro商品材質 = product.商品材質,
                                        pro商品圖片1 = prophoto.商品圖片1,
                                        pro商品圖片2 = prophoto.商品圖片2,
                                        pro商品圖片3 = prophoto.商品圖片3,
                                    };
                foreach (var item in productdetail)
                {
                    Key = (int)item.pro商品編號;
                    pdm.pro商品編號 = (int)item.pro商品編號;
                    pdm.prodetailID = item.prodetailID;
                    pdm.pro商品顏色 = item.pro商品顏色;
                    pdm.pro商品金額 = item.pro商品金額.ToString();
                    pdm.pro商品顏色圖片 = item.pro商品顏色圖片;
                    pdm.pro商品分類 = item.pro商品分類;
                    pdm.pro商品名稱 = item.pro商品名稱;
                    pdm.pro商品介紹 = item.pro商品介紹;
                    pdm.pro商品材質 = item.pro商品材質;
                    pdm.pro商品圖片1 = item.pro商品圖片1;
                    pdm.pro商品圖片2 = item.pro商品圖片2;
                    pdm.pro商品圖片3 = item.pro商品圖片3;
                }
                var totallist = from item in _context.Products
                                join prodetail in _context.ProductDetails on item.商品編號id equals prodetail.商品編號id
                                join prosize in _context.ProductsSizeDetails on prodetail.商品尺寸id equals prosize.商品尺寸id
                                join procolor in _context.ProductsColorDetails on prodetail.商品顏色id equals procolor.商品顏色id
                                where item.商品編號id == Key
                                select new
                                {
                                    pro商品顏色圖片list = procolor.商品顏色圖片,
                                    pro商品尺寸list = prosize.尺寸種類,
                                    pro商品DetailIDlist = prodetail.Id
                                };
                foreach (var CC in totallist) 
                {
                    pdm.pro商品尺寸list.Add(CC.pro商品尺寸list);
                    pdm.pro商品顏色圖片list.Add(CC.pro商品顏色圖片list);
                    pdm.pro商品DetailIDlist.Add(CC.pro商品DetailIDlist);
                }
                return View(pdm);

            }


        }
    }
}
