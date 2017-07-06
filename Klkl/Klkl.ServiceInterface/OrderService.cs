using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
    [Authenticate]
    public class OrderService:Service
    {
        public IAutoQueryDb AutoQuery { get; set; }
        public IAuthRepository AuthRepository { get; set; }
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
            var session = GetSession();
            var admin = session.HasRole("Admin", AuthRepository);
            query.And(e => admin || e.Khdb == session.DisplayName);
            var result = AutoQuery.Execute(request, query);
       

            return new { total = result.Total, result = result.Results };

        }

        public object GetOrders()
        {
            return new object();
        }

        public IList<Order> GetAllOrders()
        {
            return Db.Select<Order>(e => !e.Del);
        } 

        public IList<OrderGoods> GetAllOrderGoodses()
        {
            return Db.Select<OrderGoods>();
        } 
    
        public object Get(OrderIndex request)
        {
           // Response.Redirect("/#/page/login");
            return new object();
      
        }
        [Authenticate]
     
        public object Post(OrderDel request)
        {
            var order = Db.SingleById<Order>(request.ID);
            var session = GetSession();
            if (order==null||(session.UserAuthId != order.UserID&& !session.HasRole("Admin", AuthRepository)))
            {
                throw new Exception("不能操作");
            }
            if (!order.NeedSp && !session.HasRole("Admin", AuthRepository))
            {
                throw new Exception("已审批的订单只能由管理员删除");
            }
            //   Db.DeleteById<Order>(request.ID);
            Db.Update<Order>(new {Del = true}, e => e.ID == request.ID);
            Db.Update<OrderGoods>(new {Del = true}, e => e.OrderID == request.ID);
            Db.Update<OrderCost>(new {Del = true}, e => e.OrderID == request.ID);
            return new object();
        }

        [Authenticate]
        [RequiredRole("Admin")]

        public object Post(OrderCheck request)
        {
            var order = Db.SingleById<Order>(request.ID);
            if (!order.NeedSp)
            {
                throw new Exception("已审批");
            }
            Db.UpdateOnly(() => new Order { NeedSp = false }, e => e.ID == request.ID);
            return new object();
        }

        public object Get(GetOrder request)
        {
            GetOrderResponse response=new GetOrderResponse();
        
            response.Goodses = Db.Select<Goods>().OrderBy(e=>e.Name).ToList();
            response.Costs = Db.Select<Cost>();
            response.Customers = Db.Select<Customer>().OrderBy(e=>e.Khmc).ToList();
            response.Categories = Db.Select<Category>().OrderBy(e=>e.Name).ToList();
            response.Khdbs = Db.Select<UserAuth>();
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
                        orderGoodse.Type = goods.Type;
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
        [Authenticate]
        public object Post(UpdateOrder request)
        {
            var order = Db.SingleById<Order>(request.Order.ID);
            if (order != null && !order.NeedSp && !GetSession().HasRole("Admin", AuthRepository))
            {
                throw new HttpError("订单审批后只有总经理可以修改订单");
            }
            if (request.Order.ID>0)
            {
                Db.Update(request.Order);
            }
            else
            {
                request.Order.NeedSp = true;
                request.Order.OrderID = GetOrderNo();
               // request.Order.UserID = GetSession().UserAuthId;
                request.Order.ID = (int) Db.Insert(request.Order,true);
            }
            return new {ID = request.Order.ID, OrderID = request.Order.OrderID};
        }

        public object Post(UpdateOrderGoods request)
        {
            if (request.OrderGoods.OrderID==0)
            {
                return 0;
            }
            var order = Db.SingleById<Order>(request.OrderGoods.OrderID);
            if (order != null && !order.NeedSp  && !GetSession().HasRole("Admin", AuthRepository))
            {
                throw new HttpError("订单审批后只有总经理可以修改订单");
            }
            if (request.OrderGoods.ID>0)
            {
                Db.Update(request.OrderGoods);
            }
            else
            {
                request.OrderGoods.ID = (int) Db.Insert(request.OrderGoods,true);
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
            var order = Db.SingleById<Order>(request.OrderCost.OrderID);
            if (order != null && !order.NeedSp && !GetSession().HasRole("Admin", AuthRepository))
            {
                throw new HttpError("订单审批后只有总经理可以修改订单");
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

        [Authenticate]
        //     [RequiredRole("Admin")]
        public object Post(GetOrderReport request)
        {
            var orderSql = Db.From<Order>().Where(e => e.CreateAt >= request.Dt1 && e.CreateAt <= request.Dt2 && !e.Del);
            if (!GetSession().HasRole("Admin", AuthRepository))
            {
                var userId = GetSession().UserAuthId;
                orderSql.And<Order>(e => e.UserID == userId);
            }
            var orders= Db.Select<Order>(orderSql);
            var ordergoodses = Db.Select<OrderGoods>();
            var ordercosts = Db.Select<OrderCost>();
            var approvals = Db.Select<Approval>();
            foreach (var order in orders)
            {
                var goodses = ordergoodses.Where(e => e.OrderID == order.ID).ToList();
                var costs = ordercosts.Where(e => e.OrderID == order.ID);
                order.Zje = goodses.Sum(e => e.Amount);
                order.Yf = goodses.Sum(e => e.Fare);
                order.Dk = goodses.Sum(e => e.Shje);
                
                var approval = approvals.FirstOrDefault(e => e.OrderID == order.ID);
                order.Fy =(approval?.Je ?? 0)+ costs.Sum(e=>e.Money);
            }
            return orders;
        }

        private string GetOrderNo()
        {
            var no = Redis.Get<KeyValuePair<DateTime, int>>("orderno");
            if (no.Key != DateTime.Now.Date)
            {
                no = new KeyValuePair<DateTime, int>(DateTime.Now.Date, 1);
            }
            else
            {
                no = new KeyValuePair<DateTime, int>(DateTime.Now.Date, no.Value + 1);
            }
            Redis.Set("orderno", no);
            var orderno = $"{DateTime.Now.ToString("yyyyMMdd")}{$"{no.Value:000}"}";
            return orderno;
        }
    }
}
