using Demo.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Models
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DateTime MDate { get; set; }
        public int DueDays { get; set; }
        public DateTime DueDate { get; set; }
        public string Party { get; set; }
        public List<ItemDetail> OrderDetails { get; set; }
    }
}
