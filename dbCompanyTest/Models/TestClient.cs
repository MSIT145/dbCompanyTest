using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dbCompanyTest.Models
{
    public partial class TestClient
    {
        public TestClient()
        {
            Offers = new HashSet<Offer>();
            Orders = new HashSet<Order>();
            會員商品暫存s = new HashSet<會員商品暫存>();
        }

        public string 客戶編號 { get; set; } = null!;
        [Required]
        public string? 客戶姓名 { get; set; }
        [RegularExpression("^09[0-9]{8}$")]
        public string? 客戶電話 { get; set; }
        [RegularExpression("^[A-Z]{1}[1-2]{1}[0-9]{8}$")]
        public string? 身分證字號 { get; set; }
        public string? 縣市 { get; set; }
        public string? 區 { get; set; }
        [Required]
        public string? 地址 { get; set; }
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$")]
        public string? Email { get; set; }
        [Required]
        public string? 密碼 { get; set; }
        [Required]
        public string? 性別 { get; set; }
        [Required]
        public string? 生日 { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<會員商品暫存> 會員商品暫存s { get; set; }
    }
}
