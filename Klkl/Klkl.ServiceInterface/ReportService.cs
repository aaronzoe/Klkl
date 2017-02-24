using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Web;

namespace Klkl.ServiceInterface
{
    [Authenticate]
   public class ReportService:Service
    {
        [Authenticate]
        // [RequiredRole("Admin","总经理")]
        public object Post(ProductReport request)
        {

            //var sql = Db.From<Goods>()
            //    .LeftJoin<Goods, OrderGoods>((c, o) => c.ID == o.GoodsID)
            //    .LeftJoin<Order, OrderGoods>((o, g) => o.ID == g.OrderID)
            //    .Where<Order>(o=>o.CreateAt>request.Dt1&&o.CreateAt<request.Dt2)
            //    .Select<Goods, Order, OrderGoods>((c, o, g) => new
            //    {
            //        c.ID,
            //        c.Category,
            //        c.Name,
            //        c.Size,
            //        SellNum = Sql.As(Sql.Sum(g.Num), "SellNum"),
            //        SellAmount = Sql.As(Sql.Sum(g.Amount), "SellAmount"),
            //    }).GroupBy(c => new
            //    {
            //        c.ID,
            //        c.Category,
            //        c.Name,
            //        c.Size
            //    }).OrderBy(g=>g.Name);

            //var cs = Db.Select(sql);
            //return cs;

            var orderSql = Db.From<Order>().Where(o => o.CreateAt > request.Dt1 && o.CreateAt < request.Dt2).Select(o => o.ID);
            if (!GetSession().HasRole("Admin", AuthRepository))
            {
                var userId =GetSession().UserAuthId;
                orderSql.And(e => e.UserID == userId);
            }
            var sql = Db.From<Goods>()
              .LeftJoin<Goods, OrderGoods>((c, g) => c.ID == g.GoodsID && Sql.In(g.OrderID, orderSql))
            
              .Select<Goods,  OrderGoods>((c,  g) => new
              {
                  c.ID,
                  c.Category,
                  c.Name,
                  c.Size,
                  SellNum = Sql.As(Sql.Sum(g.Num), "SellNum"),
                  SellAmount = Sql.As(Sql.Sum(g.Amount), "SellAmount"),
              }).GroupBy(c => new
              {
                  c.ID,
                  c.Category,
                  c.Name,
                  c.Size
              }).OrderBy(g => g.Name);
            
          

            var cs = Db.Select<GoodReportModel>(sql);
            return cs;

        }
        [Authenticate]
        //[RequiredRole("Admin", "总经理")]
        public object Post(CustomerReport request)
        {
         var sql=   Db.From<Customer>()
                .LeftJoin<Customer, Order>((c, o) => c.Khmc == o.Khmc&&o.CreateAt> request.Dt1&&o.CreateAt<request.Dt2)
                .LeftJoin<Order, OrderGoods>((o, g) => o.ID == g.OrderID).Select<Customer,Order, OrderGoods>((c,o,g)=>new
                {
                    c.ID,
                    c.Khqd,
                    c.Lxr,
                    c.Lxdh,
                    c.Shdz,
                    c.Qy,
                    c.Khmc,
                    OrderNum =  Sql.As( Sql.Count(o.ID),"OrderNum"),
                    OrderAmount =  Sql.As( Sql.Sum(g.Amount), "OrderAmount"),
                }).GroupBy(c => new{
                    c.ID,
                    c.Khqd,
                    c.Lxr,
                    c.Lxdh,
                    c.Shdz,
                    c.Qy,
                    c.Khmc
                }).OrderBy(c=>c.Khmc);

     
            if (!GetSession().HasRole("Admin", AuthRepository))
            {
                var userId = GetSession().UserAuthId;
                sql.And<Order>(e => e.UserID == userId);
            }
            var cs=   Db.Select<CustomerReportModel>(sql);
            return cs;
        }
    }
}
