using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace dbCompanyTest.Controllers
{
    public class ProductController : Controller
    {
        dbCompanyTestContext db = new dbCompanyTestContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _CreateDetail()
        {

            return PartialView();
        }
        public IActionResult CreateProduct(string id,string time,string name,string price,string introduce ,string material,string weight,string cost ,string typeid,string shoeid,string instock,string onshelves)
        {
            //可以先做後端檢查(time、價格、成本不能亂填)


            //檢查後可以寫入Product Model內存入資料庫
            try
            {
                Product pro = new Product();
                pro.上架時間 = time;
                pro.商品名稱 = name;
                pro.商品價格 = Convert.ToDecimal(double.TryParse(price, out double _price) ? _price : 0);
                pro.商品介紹 = introduce;
                pro.商品材質 = material;
                pro.商品重量 = Int32.TryParse(weight, out int _weight) ? _weight : 0;
                pro.商品成本 = Convert.ToDecimal(double.TryParse(cost, out double _cost) ? _cost : 0);
                pro.商品分類id = Int32.TryParse(typeid, out int _typeid) ? _typeid : 0;
                pro.商品鞋種id = Int32.TryParse(shoeid, out int _shoeid) ? _shoeid : 0;
                pro.商品是否有貨 = bool.TryParse(instock, out bool _instock) ? _instock : false;
                pro.商品是否上架 = bool.TryParse(onshelves, out bool _onshelves) ? _onshelves : false;

                db.Products.Add(pro);
                db.SaveChanges();
                return Json("新增成功");
            }
            catch
            {
                return Json("失敗");
            }
        }

        public IActionResult Pro_Edit(string id) {
            if (!string.IsNullOrEmpty(id))
            {
                var pro = db.Products.Where(p => p.商品編號id == Int32.Parse(id)).FirstOrDefault();
                if (pro != null)
                {
                    return Json(pro);
                }
                return Json(null);
            }             
            return Json(null);        
        }

        public IActionResult GetTyteD()
        {
            IEnumerable<ProductsTypeDetail> data = db.ProductsTypeDetails.ToList();
            return Json(data);
        }

        public IActionResult GetShoe()
        {
            IEnumerable<商品鞋種> data = db.商品鞋種s.ToList(); 
            return Json(data);
        }


        [HttpGet]
        public IActionResult showlist()
        {
            var data =from  p in db.Products.ToList()
                      join pt in db.ProductsTypeDetails on p.商品分類id equals pt.商品分類id
                      join s in db.商品鞋種s on p.商品鞋種id equals s.商品鞋種id
                      select new BACK_ProductViewModels 
                      {
                            商品編號id  = p.商品編號id,
                            上架時間 =p.上架時間,
                            商品名稱 = p.商品名稱,
                            商品價格 = p.商品價格,
                            商品介紹 = p.商品介紹,
                            商品材質  = p.商品材質,
                            商品重量  = p.商品重量,
                            商品成本 = p.商品成本,
                            商品是否有貨 = p.商品是否有貨,
                            商品分類  = pt.商品分類名稱,
                            商品鞋種 = s.鞋種,
                            商品是否上架  = p.商品是否上架
                       };

            return Json(new { data });
        }

        [HttpGet]
        public IActionResult showDetail(string id) 
        {
            if (!string.IsNullOrEmpty(id))
            {
                int PDid = Convert.ToInt32(id);
                var data = from pd in db.ProductDetails.ToList()
                           join z in db.ProductsSizeDetails on pd.商品尺寸id equals z.商品尺寸id
                           join c in db.ProductsColorDetails on pd.商品顏色id equals c.商品顏色id
                           join i in db.圖片位置s on pd.圖片位置id equals i.圖片位置id
                           where pd.商品編號id == PDid
                           select new Back_ProducDetail
                           {
                               明細編號 = pd.Id.ToString(),
                               明細尺寸 = z.尺寸種類,
                               顏色 = c.商品顏色種類,
                               數量 = pd.商品數量,
                               商品圖片1 = i.商品圖片1,
                               商品圖片2 = i.商品圖片2,
                               商品圖片3 = i.商品圖片3,
                               是否上架 = pd.商品是否上架,
                               是否有貨 = pd.商品是否有貨
                           };
                return Json(new { data });
            }
            return View();
        }
    }
}
