namespace dbCompanyTest.ViewModels
{
    public class BACK_ProductViewModels
    {
        public int 商品編號id { get; set; }
        public string? 上架時間 { get; set; }
        public string? 商品名稱 { get; set; }
        public decimal? 商品價格 { get; set; }
        public string? 商品介紹 { get; set; }
        public string? 商品材質 { get; set; }
        public int? 商品重量 { get; set; }
        public decimal? 商品成本 { get; set; }
        public bool? 商品是否有貨 { get; set; }
        public string? 商品分類 { get; set; }
        public string? 商品鞋種 { get; set; }
        public bool? 商品是否上架 { get; set; }

        public IFormFile excel { get; set; }

        public string? fileName { get; set; }
    }

    public class Back_GetProName
    {
        public int 商品編號id { get; set; }

        public string? 商品名稱 { get; set; }
    }

    public class Back_ProducDetail
    {
        public string? id { get; set; }
        public string? 商品編號id { get; set; }
        public string? 明細尺寸 { get; set; }
        public string? 顏色 { get; set; }
        public string? 數量 { get; set; }
        public string? 明細編號 { get; set; }
        public string? 商品圖片1 { get; set; }
        public string? 商品圖片2 { get; set; }
        public string? 商品圖片3 { get; set; }
        public string? 圖片位置id { get; set; }
        public string? 商品是否有貨 { get; set; }
        public string? 商品是否上架 { get; set; }
      
    }

    public class Product_CDictionary
    {        
        public static readonly string SK_SEARCH_PRODUCTS_LIST = "SK_SEARCH_PRODUCTS_LIST";
    }

    public class Back_Product_library
    {
        public  String RandomString(int length)
        {
            //少了英文的IO和數字10，要避免使用者判斷問題時會使用到
            string allChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            //string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";//26個英文字母
            char[] chars = new char[length];
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            rd.Next();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allChars[rd.Next(0, allChars.Length)];
            }

            return new string(chars);
        }

    }

}
