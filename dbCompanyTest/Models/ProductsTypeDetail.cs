using System;
using System.Collections.Generic;

namespace dbCompanyTest.Models
{
    public partial class ProductsTypeDetail
    {
        public ProductsTypeDetail()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int 商品分類id { get; set; }
        public int? 商品鞋種id { get; set; }
        public string? 商品分類名稱 { get; set; }

        public virtual 商品鞋種? 商品鞋種 { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
