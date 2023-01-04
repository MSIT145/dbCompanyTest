using System;
using System.Collections.Generic;

namespace dbCompanyTest.Models
{
    public partial class ToDoList
    {
        public int 交辦事項id { get; set; }
        public string 員工編號 { get; set; } = null!;
        public string 交辦內容 { get; set; } = null!;
        public string 批示 { get; set; } = null!;
        public string 狀態 { get; set; } = null!;

        public virtual TestStaff 員工編號Navigation { get; set; } = null!;
    }
}
