using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
    public class OrderService:Service
    {
        public IAutoQuery AutoQuery { get; set; }
        public object Post(OrderList request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            var result = AutoQuery.Execute(request, query);
            return result;

        }

        public object Get(OrderList request)
        {
            var orders= Db.Select<Order>();
            return orders;

        }
    }
}
