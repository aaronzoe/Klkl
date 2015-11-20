using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/order/index")]
    public class OrderIndex
    { }

    [Route("/order/list")]
    public class OrderList : QueryBase<Order>,IReturn<HelloResponse>
    {
     
    }

    public class OrderListResponse
    {
        public string Result { get; set; }
    }
    [Route("/order/del","POST")]
    public class OrderDel
    {
        public int ID { get; set; }
    }

    [Route("/order/{id}","GET")]
    public class GetOrder
    {
        public int ID { get; set; }
    }
    [Route("/order/update")]
    public class UpdateOrder
    {
        public Order Order { get; set; }
    }
    [Route("/order/updateordergoods")]
    public class UpdateOrderGoods
    {
        public OrderGoods OrderGoods { get; set; }
    }
}
