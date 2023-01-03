using System;
using System.Collections.Generic;

namespace dbCompanyTest.Models
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? 商品編號id { get; set; }
        public int? 商品尺寸id { get; set; }
        public int? 商品顏色id { get; set; }
        public int? 商品數量 { get; set; }
        public int? 商品分類id { get; set; }
        public int? 商品鞋種id { get; set; }
        public string? 商品編號 { get; set; }
        public int? 圖片位置id { get; set; }

        public virtual ProductsTypeDetail? 商品分類 { get; set; }
        public virtual ProductsSizeDetail? 商品尺寸 { get; set; }
        public virtual Product? 商品編號Navigation { get; set; }
        public virtual 商品鞋種? 商品鞋種 { get; set; }
        public virtual ProductsColorDetail? 商品顏色 { get; set; }
        public virtual 圖片位置? 圖片位置 { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
