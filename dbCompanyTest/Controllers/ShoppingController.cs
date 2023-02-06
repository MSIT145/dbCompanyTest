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
        dbCompanyTestContext _context = new dbCompanyTestContext();
        //string name = "CL1-0005891";
        //private readonly dbCompanyTestContext _context;

        //public ShoppingController(dbCompanyTestContext context)
        //{
        //    _context = context;
        //}

        // GET: Shopping
        public IActionResult Index()
        {
            List<會員商品暫存>? carSession = null;
            string json = "";
            //判斷是否登入
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                //取得Login Session
                string login = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
                var user = JsonSerializer.Deserialize<TestClient>(login);

                var dbCompanyTestContext = _context.會員商品暫存s.Select(x => x).Where(c => c.購物車或我的最愛 == true && c.客戶編號.Contains(user.客戶編號)).ToList();
                //是否有car session
                if (!HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION))
                {
                    //沒有car session
                    //是否有dbCompanyTestContext
                    if (dbCompanyTestContext.Count != 0)
                    {
                        //有dbCompanyTestContext
                        json = JsonSerializer.Serialize(dbCompanyTestContext);
                        HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, json);
                    }
                }
                else
                {
                    //有car session
                    carSession = new List<會員商品暫存>();
                    json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                    carSession = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                }
            }
            else return RedirectToAction("Login", "Login");
            return View(carSession);
        }
        public IActionResult joinSQLToSession()
        {
            List<會員商品暫存>? carSession = null;
            string json = "";
            //判斷是否登入
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                //取得Login Session
                string login = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
                var user = JsonSerializer.Deserialize<TestClient>(login);

                var dbCompanyTestContext = _context.會員商品暫存s.Select(x => x).Where(c => c.購物車或我的最愛 == true && c.客戶編號.Contains(user.客戶編號)).ToList();
                //是否有car session
                if (!HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION))
                {
                    //沒有car session
                    //是否有dbCompanyTestContext
                    if (dbCompanyTestContext.Count != 0)
                    {
                        //有dbCompanyTestContext
                        json = JsonSerializer.Serialize(dbCompanyTestContext);
                        HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, json);
                    }
                }
                else
                {
                    //有car session
                    carSession = new List<會員商品暫存>();
                    json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                    carSession = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                    foreach (會員商品暫存 x in dbCompanyTestContext)
                    {
                        carSession.Add(x);
                        json = JsonSerializer.Serialize(carSession);
                        HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, json);
                    }
                }
            }
                return Content("加入");
        }
        public IActionResult GetCarJson()
        {
            var json = "";
            List<會員商品暫存>? carSession = null;
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION))
            {
                json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                carSession = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
            }
            else
                json = "NO";

            return Json(carSession);
        }
        public IActionResult GetDeliveryMony(string OPvalue)
        {
            if (string.IsNullOrEmpty(OPvalue))
                OPvalue = "0";

            return Content($"{OPvalue}", "text/plain", Encoding.UTF8);
        }

        public IActionResult DeleteCarSession(int? id)
        {
            if (id != null)
            {
                var json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
                var datas = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
                var data = datas.FirstOrDefault(x => x.Id == id);
                datas.Remove(data);
                json = JsonSerializer.Serialize(datas);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION, json);
                return Content("刪除");
            }
            else
            {
                return Content("NO");
            }
        }

        public IActionResult LoadClientDital()
        {
            var json = "";
            TestClient? userSession = null;
            if (HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
                userSession = JsonSerializer.Deserialize<TestClient>(json);
            }
            else
                json = "NO";

            return Json(userSession);
        }
        //----訂單API----------------------
        public IActionResult TestForAPI(Order data)
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            //需填入 你的網址
            var website = $"https://localhost:7013";
            var order = new Dictionary<string, string>
        {
            //特店交易編號
            { "MerchantTradeNo",  data.訂單編號},

            //特店交易時間 yyyy/MM/dd HH:mm:ss
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

            //超商取貨
            { "LogisticsType",  "CVS"},

            //選擇超商
            { "LogisticsSubType",  "UNIMART"},

            //自訂名稱欄位3
            { "GoodsAmount",  data.總金額.Value.ToString()},//

            //自訂名稱欄位3
            { "SenderName",  "ALANANDLU"},

            //自訂名稱欄位3
            { "SenderPhone",  "0977123456"},

            //自訂名稱欄位3
            { "SenderCellPhone",  "0977123456"},

            //自訂名稱欄位3
            { "ReceiverName",  data.收件人名稱},//

            //自訂名稱欄位3
            { "ReceiverPhone",  data.收件人電話},//

            //自訂名稱欄位3
            { "ReceiverCellPhone",  data.收件人電話},//

            //自訂名稱欄位3
            { "ReceiverStoreID",  "131386"},

            //自訂名稱欄位4
            { "ServerReplyURL",  $"{website}/Home/Index"},

            //特店編號， 2000132 測試綠界編號
            { "MerchantID",  "2000132"},

            //CheckMacValue 加密類型 固定填入 1 (SHA256)
            { "EncryptType",  "1"}
        };
            order["CheckMacValue"] = (new APIViewModels()).GetCheckMacValueForOrder(order);
            var HttpContent = new FormUrlEncodedContent(order);
            string response = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://logistics-stage.ecpay.com.tw/Express/Create");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage httpResponse = client.PostAsync("", HttpContent).Result;
                return Json(httpResponse.Content.ReadAsStringAsync().Result);
            }
        }
        //----訂單API結束-------------------


        //----繳費API-------------------
        public IActionResult TestforPay(Order data)
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            //需填入 你的網址
            var website = $"https://localhost:7100";

            var order = new Dictionary<string, string>
        {
            //特店交易編號
            { "MerchantTradeNo",  data.訂單編號},

            //特店交易時間 yyyy/MM/dd HH:mm:ss
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

            //交易金額
            { "TotalAmount",  data.總金額.Value.ToString()},

            //交易描述
            { "TradeDesc",  "無"},

            //商品名稱
            { "ItemName",  data.訂單編號},

            //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
            { "ExpireDate",  "3"},

            //自訂名稱欄位1
            { "CustomField1",  ""},

            //自訂名稱欄位2
            { "CustomField2",  ""},

            //自訂名稱欄位3
            { "CustomField3",  ""},

            //自訂名稱欄位4
            { "CustomField4",  ""},

            //綠界回傳付款資訊的至 此URL
            { "ReturnURL",  $"{website}/api/API/AddPayInfo"},

            //使用者於綠界 付款完成後，綠界將會轉址至 此URL
            { "OrderResultURL", $"{website}/Shopping/Index"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
            { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
            { "ClientRedirectURL",  $"{website}/Ecpay/AccountInfo/{orderId}"},

            //特店編號， 2000132 測試綠界編號
            { "MerchantID",  "2000132"},

            //忽略付款方式
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

            //交易類型 固定填入 aio
            { "PaymentType",  "aio"},

            //選擇預設付款方式 固定填入 ALL
            { "ChoosePayment",  "ALL"},

            //CheckMacValue 加密類型 固定填入 1 (SHA256)
            { "EncryptType",  "1"},
        };

            //檢查碼
            order["CheckMacValue"] = (new APIViewModels()).GetCheckMacValuecon(order);

            return View(order);

        }
        //----繳費API結束--------------


        public IActionResult CreateOrder(Order order)
        {
            dbCompanyTestContext _context = new dbCompanyTestContext();
            Order data = order;
            _context.Orders.AddRange(data);
            _context.SaveChanges();
            return Content("成功");
        }
        public IActionResult CreateOrderDital(OrderDetail order)
        {
            List<會員商品暫存>? carSession = null;
            string json = "";
            dbCompanyTestContext _context = new dbCompanyTestContext();
            OrderDetail data = order;
            carSession = new List<會員商品暫存>();
            json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_SHOPPING_CAR_SESSION);
            carSession = JsonSerializer.Deserialize<List<會員商品暫存>>(json);
            foreach (會員商品暫存 x in carSession)
            {
                data.總金額 = x.商品價格;
                data.商品數量 = x.訂單數量;
                //----因為是垃圾資料所以先用4
                //data.Id= x.商品編號;
                data.Id=4;
                //----
                data.商品價格=x.商品價格;
                data.無用id=0;
                _context.OrderDetails.AddRange(data);
            _context.SaveChanges();
            }
            
            return Content("成功");
        }
        private bool 會員商品暫存Exists(int id)
        {
            return _context.會員商品暫存s.Any(e => e.Id == id);
        }
    }
}
