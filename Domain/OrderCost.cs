using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Domain
{
    public class OrderCost
    {
        [AutoIncrement]
        public int ID { get; set; }
        public bool Del { get; set; }
        public int CostID { get; set; }
        public int OrderID { get; set; }
        public decimal Money { get; set; }
        public DateTime CreateAt { get; set; }
        [Ignore]
        public string Name { get; set; }
        public OrderCost()
        {
            CreateAt=DateTime.Now;
        }

    }
}
