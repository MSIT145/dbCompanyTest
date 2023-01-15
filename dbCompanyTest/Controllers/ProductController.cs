using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.Util;
using System.Data;
using System.Xml.Linq;
using System.ComponentModel;

namespace dbCompanyTest.Controllers
{
    public class ProductController : Controller
    {
        dbCompanyTestContext db = new dbCompanyTestContext();
        private IWebHostEnvironment _eviroment; //宣告取域 環境變數
        public ProductController(IWebHostEnvironment eviroment)  //建構子，將環境變數注入
        {
            _eviroment = eviroment;
        }
        //存放Product的資料，搜尋請改用Session
        static List<Product> searchData = new List<Product>();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(BACK_ProductViewModels vc)
        {
            if (vc.excel != null)
            {
                string excelname = vc.excel.Name;
                string oldPath = _eviroment.WebRootPath + "/Datas/" + excelname;


                System.IO.File.Delete(oldPath);   //刪掉原本的檔案

                //string photoName = $"{Guid.NewGuid().ToString()}.jpg";                   

                string path01 = _eviroment.WebRootPath + "/Datas/" + excelname;  //用環境變數取得 IIS路徑(wwwroot)
                using (FileStream fs = new FileStream(path01, FileMode.Create))
                {
                    vc.excel.CopyTo(fs);   //將檔案寫入指定路徑      
                }
                using (FileStream fs = new FileStream(path01, FileMode.Open))
                {
                    method_uploadEx(fs, vc.excel);
                }
            }
            return RedirectToAction("Index");
        }

        //將檔案與資料流轉成DataTable並存入資料庫
        public void method_uploadEx(Stream stream, IFormFile formFile)
        {
            DataTable dataTable = new DataTable();
            IWorkbook wb;
            ISheet sheet;
            IRow headerRow;
            int cellCount; //紀錄共有幾欄
            try
            {
                //依excel版本，NPOI載入檔案
                if (formFile.FileName.ToUpper().EndsWith("XLSX"))
                    wb = new XSSFWorkbook(stream); // excel版本(.xlsx)
                else
                    wb = new HSSFWorkbook(stream); // excel版本(.xls)

                //取第一個頁籤   
                sheet = wb.GetSheetAt(0);

                //取第一個頁籤的第一列
                headerRow = sheet.GetRow(0);

                //計算出第一列共有多少欄位
                cellCount = headerRow.LastCellNum;

                //迴圈執行第一列的第一個欄位到最後一個欄位，將抓到的值塞進DataTable做完欄位名稱
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    dataTable.Columns.Add(new DataColumn(headerRow.GetCell(i).StringCellValue));
                }

                //int j; //計算每一列讀到第幾個欄位
                int column = 1; //計算每一列讀到第幾個欄位

                // 略過第零列(標題列)，一直處理至最後一列
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    //取目前的列(row)
                    IRow row = sheet.GetRow(i);

                    //若該列的第一個欄位無資料，break跳出
                    if (string.IsNullOrEmpty(row.Cells[0].ToString().Trim()))
                    {
                        break;
                    }

                    //宣告DataRow
                    DataRow dataRow = dataTable.NewRow();
                    //宣告ICell
                    ICell cell;

                    try
                    {
                        //依先前取得，依每一列的欄位數，逐一設定欄位內容
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            //計算每一列讀到第幾個欄位(秀在錯誤訊息上)
                            column = j + 1;

                            //設定cell為目前第j欄位
                            cell = row.GetCell(j);

                            if (cell != null) //若cell有值
                            {
                                //用cell.CellType判斷資料的型別
                                //再依照欄位屬性，用StringCellValue、DateCellValue、NumericCellValue、DateCellValue取值
                                switch (cell.CellType)
                                {
                                    //字串型態欄位
                                    case NPOI.SS.UserModel.CellType.String:
                                        //設定dataRow第j欄位的值，cell以字串型態取值
                                        dataRow[j] = cell.StringCellValue;
                                        break;

                                    //數字型態欄位
                                    case NPOI.SS.UserModel.CellType.Numeric:

                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) //日期格式
                                        {
                                            //設定dataRow第j欄位的值，cell以日期格式取值
                                            dataRow[j] = DateTime.FromOADate(cell.NumericCellValue).ToString("yyyy/MM/dd HH:mm");
                                        }
                                        else //非日期格式
                                        {
                                            //設定dataRow第j欄位的值，cell以數字型態取值
                                            dataRow[j] = cell.NumericCellValue;
                                        }
                                        break;

                                    //布林值
                                    case NPOI.SS.UserModel.CellType.Boolean:

                                        //設定dataRow第j欄位的值，cell以布林型態取值
                                        dataRow[j] = cell.BooleanCellValue;
                                        break;

                                    //空值
                                    case NPOI.SS.UserModel.CellType.Blank:

                                        dataRow[j] = "";
                                        break;

                                    // 預設
                                    default:

                                        dataRow[j] = cell.StringCellValue;
                                        break;
                                }
                            }
                        }
                        //DataTable加入dataRow
                        dataTable.Rows.Add(dataRow);
                    }
                    catch (Exception ex)
                    {
                        //錯誤訊息
                        //throw new Exception("第 " + i + "列第" + column + "欄，資料格式有誤:\r\r" + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                //釋放資源
                sheet = null;
                wb = null;
                stream.Dispose();
                stream.Close();
            }

            //dataTable跑回圈，insert資料至DB
            foreach (DataRow dr in dataTable.Rows)
            {
                Product x = new Product();
                x.上架時間 = dr[1].ToString(); 
                x.商品名稱 = dr[2].ToString(); 
                x.商品價格 = Convert.ToDecimal(double.TryParse(dr[3].ToString(), out double _price) ? _price : 0);
                x.商品介紹 = dr[4].ToString();
                x.商品材質 = dr[5].ToString();
                x.商品重量 = Int32.TryParse(dr[6].ToString(), out int _weight) ? _weight : 0;
                x.商品成本 = Convert.ToDecimal(double.TryParse(dr[7].ToString(), out double _cost) ? _cost : 0);
                x.商品分類id = Int32.TryParse(dr[8].ToString(), out int _typeid) ? _typeid : 0;
                x.商品鞋種id = Int32.TryParse(dr[9].ToString(), out int _shoeid) ? _shoeid : 0;
                x.商品是否有貨 = bool.TryParse(dr[10].ToString(), out bool _instock) ? _instock : false;
                x.商品是否上架 = bool.TryParse(dr[11].ToString(), out bool _onshelves) ? _onshelves : false;

                try
                {
                    
                    db.Products.Add(x);
                    db.SaveChanges();
                    //Response.BodyWriter("<script language=javascript>alert('檔案匯入成功');</" + "script>");
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }

        //下載
        public IActionResult Downloads(BACK_ProductViewModels vc)
        {
            //取得欄位名稱
            DataTable dt = ConvertToDataTable(searchData.ToArray());
            string path = DataTableToExcelFile(dt);

            //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string contentType = "application/vnd.ms-excel";
            string fileName = $"{vc.fileName}.xls";
            //寫入檔案

            //FileStream fs = new FileStream(path, FileMode.Open);     
            var stream = System.IO.File.OpenRead(path);  //創建資料流
            return File(stream, contentType, fileName);  //資料流是否要關閉

        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
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
            ISheet ws;

            ////建立Excel 2007檔案
            //IWorkbook wb = new XSSFWorkbook();
            //ISheet ws;

            if (dt.TableName != string.Empty)
            {
                ws = wb.CreateSheet(dt.TableName);
            }
            else
            {
                ws = wb.CreateSheet("Sheet1");
            }

            ws.CreateRow(0);//第一行為欄位名稱
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ws.GetRow(0).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ws.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ws.GetRow(i + 1).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            //string filename = DateTime.Now.ToString("yyyyMMddHHmmss");
            string filename = "TempUp";
            string path = _eviroment.WebRootPath + "/Datas/" + $"{filename}.xls";  //用環境變數取得 IIS路徑(wwwroot)
            System.IO.File.Delete(path);   //刪掉原本的檔案           
            FileStream file = new FileStream(path, FileMode.Create);//產生檔案
            wb.Write(file);
            file.Close();
            return path;
        }




        public IActionResult _CreateDetail()
        {

            return PartialView();
        }

        public IActionResult DeleteProduct(string? id)
        {
            int _id = 0;
            if (string.IsNullOrEmpty(id)|| !(Int32.TryParse(id, out _id)))
                return Json("錯誤_傳輸id資料異常");

            IEnumerable<ProductDetail> data = db.ProductDetails.Where(pd => pd.商品編號id == _id).ToList();
            if(data.Count() !=0)
                return Json("警告_商品尚有明細,不能刪除");

            var Pro = db.Products.FirstOrDefault(p => p.商品編號id == _id);
            if(Pro == null)
                return Json("錯誤_沒有此項商品");
            string name = Pro.商品名稱;
            db.Products.Remove(Pro);
            db.SaveChanges();
        
            return Json($"商品{name}刪除成功");
        }


        public IActionResult EditProduct(string id, string time, string name, string price, string introduce, string material, string weight, string cost, string typeid, string shoeid, string instock, string onshelves)
        {
            //可以先做後端檢查(time、價格、成本不能亂填)


            //檢查後可以寫入Product Model內存入資料庫
            int _id = 0;

            if (string.IsNullOrEmpty(id)|| !(Int32.TryParse(id,out _id)))
                return Json("失敗!商品id沒資料!!");

            try
            {
                var pro = db.Products.FirstOrDefault(p => p.商品編號id == _id);
                if (pro == null)
                    return Json("失敗!找不到資料");
                
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
                    db.SaveChanges();
                    return Json("修改成功!");
            }
            catch
            {
                return Json("失敗!資料庫寫入異常");
            }
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
