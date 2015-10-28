
using ServiceStack.DataAnnotations;

namespace Domain
{
    public class Order
    {
        [AutoIncrement]
        public int ID { get; set; }
        public string OrderID { get; set; }

        public string Khmc { get; set; }
        public string Khqd { get; set; }
        public string Lxr { get; set; }
        public string Lxdh { get; set; }
        public string AreaName { get; set; }
        public string Shdz { get; set; }
      
    }
}
