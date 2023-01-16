using System.ComponentModel.DataAnnotations;

namespace dbCompanyTest.Hubs
{
    public class test
    {
        public string 客戶編號 { get; set; } = null!;
        public string? 客戶姓名 { get; set; }
        public string? 客戶電話 { get; set; }
        public string? 身分證字號 { get; set; }
        public string? 縣市 { get; set; }
        public string? 區 { get; set; }
        public string? 地址 { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? 密碼 { get; set; }
        public string? 性別 { get; set; }
        public string? 生日 { get; set; }

        public string? name { get; set; }   
    }
}
