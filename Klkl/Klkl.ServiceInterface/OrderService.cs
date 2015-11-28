using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
    [Authenticate]
    public class OrderService:Service
    {
        public IAutoQuery AutoQuery { get; set; }
        public object Post(OrderList request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams()).And(e=>!e.Del);
            var result = AutoQuery.Execute(request, query);
         //   return result;
            return new {total = result.Total, result = result.Results};

        }
        [Authenticate]
        public object Get(OrderList request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams()).And(e => !e.Del);
            var result = AutoQuery.Execute(request, query);
            return new { total = result.Total, result = result.Results };

        }
    
        public object Get(OrderIndex request)
        {
           // Response.Redirect("/#/page/login");
            return new object();
      
        }

        public object Post(OrderDel request)
        {
         //   Db.DeleteById<Order>(request.ID);
            Db.Update<Order>(new {Del = true}, e => e.ID == request.ID);
            return new object();
        }

        public object Get(GetOrder request)
        {
            GetOrderResponse response=new GetOrderResponse();
        
            response.Goodses = Db.Select<Goods>();
            response.Costs = Db.Select<Cost>();
            response.Customers = Db.Select<Customer>();
            response.Categories = Db.Select<Category>();
            if (request.ID==0)
            {
                response.Order=new Order() {OrderGoodses = new List<OrderGoods>()};
            }
            else
            {
                response.Order = Db.SingleById<Order>(request.ID);
                response.Order.OrderGoodses = Db.Select<OrderGoods>(e => e.OrderID == request.ID);
                response.Order.OrderCosts = Db.Select<OrderCost>(e => e.OrderID == request.ID);
                foreach (var orderGoodse in response.Order.OrderGoodses)
                {
                    var goods = response.Goodses.FirstOrDefault(e => e.ID == orderGoodse.GoodsID);
                    if (goods!=null)
                    {
                        orderGoodse.Category = goods.Category;
                        orderGoodse.Name = goods.Name;
                        orderGoodse.Size = goods.Size;
                    }
                }
                foreach (var orderCost in response.Order.OrderCosts)
                {
                    var cost = response.Costs.FirstOrDefault(e => e.ID == orderCost.CostID);
                    if (cost!=null)
                    {
                        orderCost.Name = cost.Name;
                    }
                }
            }
    
            return response;
        }

        public object Post(UpdateOrder request)
        {
            if (request.Order.ID>0)
            {
                Db.Update(request.Order);
            }
            else
            {
                request.Order.ID = (int) Db.Insert(request.Order);
            }
            return request.Order.ID;
        }

        public object Post(UpdateOrderGoods request)
        {
            if (request.OrderGoods.OrderID==0)
            {
                return 0;
            }
            if (request.OrderGoods.ID>0)
            {
                Db.Update(request.OrderGoods);
            }
            else
            {
                request.OrderGoods.ID = (int) Db.Insert(request.OrderGoods);
            }
            return request.OrderGoods.ID;
        }

        public object Post(DelOrderGoods request)
        {
            return Db.DeleteById<OrderGoods>(request.ID);
        }

        public object Post(UpdateOrderCost request)
        {
            if (request.OrderCost.OrderID == 0)
            {
                return 0;
            }
            if (request.OrderCost.ID > 0)
            {
                Db.Update(request.OrderCost);
            }
            else
            {
                request.OrderCost.ID = (int)Db.Insert(request.OrderCost);
            }
            return request.OrderCost.ID;
        }

        public object Post(DelOrderCost request)
        {
            return Db.DeleteById<OrderCost>(request.ID);
        }


        [RequiredRole("Admin")]
        public object Post(GetOrderReport request)
        {
            var orders= Db.Select<Order>(e=>e.CreateAt>=request.Dt1&&e.CreateAt<=request.Dt2);
            var ordergoodses = Db.Select<OrderGoods>();
            var approvals = Db.Select<Approval>();
            foreach (var order in orders)
            {
                var goodses = ordergoodses.Where(e => e.OrderID == order.ID).ToList();
                order.Zje = goodses.Sum(e => e.Amount);
                order.Yf = goodses.Sum(e => e.Fare);
                order.Dk = goodses.Sum(e => e.Shje);
                var approval = approvals.FirstOrDefault(e => e.OrderID == order.ID);
                order.Fy = approval?.Je ?? 0;
            }
            return orders;
        }
    }
}
