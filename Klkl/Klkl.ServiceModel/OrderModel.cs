using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/order/list")]
    public class OrderList : QueryBase<Order>,IReturn<HelloResponse>
    {
     
    }

    public class OrderListResponse
    {
        public string Result { get; set; }
    }
}
