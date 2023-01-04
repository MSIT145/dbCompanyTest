using System;
using System.Collections.Generic;

namespace dbCompanyTest.Models
{
    public partial class 商品鞋種
    {
        public 商品鞋種()
        {
            ProductsTypeDetails = new HashSet<ProductsTypeDetail>();
        }

        public int 商品鞋種id { get; set; }
        public string? 鞋種 { get; set; }

        public virtual ICollection<ProductsTypeDetail> ProductsTypeDetails { get; set; }
    }
}
