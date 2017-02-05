using Klkl.ServiceModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using Domain;
using System.Data.SqlClient;
using ServiceStack.Auth;

namespace Klkl.ServiceInterface
{
    public class MemberReportService : Service
    {
        public MemberReportHpIndexResponse Get(MemberReportHp request)
        {
            var customers = Db.Select<Customer>(Db.From<Customer>().Select(e =>new { e.Khmc ,e.ID}).OrderBy(e =>new { e.Khmc}));
            var users = Db.Select<UserAuth>(Db.From<UserAuth>().Select(e =>new { e.DisplayName ,e.Id}).OrderBy(e =>new { e.DisplayName}));
            return new MemberReportHpIndexResponse() { Customers=customers,Users=users};
        }
        [Authenticate]
        public object Post(MemberReportHp request)
        {
            //var sql = Db.From<Goods>().LeftJoin<OrderGoods>((g, o) => g.ID == o.GoodsID && !o.Del)
            //    .LeftJoin<Order,OrderGoods >(( o,g) => g.OrderID == o.ID)
            //    .LeftJoin<Order,OrderCost >(( o,g) => g.OrderID == o.ID)

            //    .Where(e => e.Type==2)
            //    .And<Order>(e => e.CreateAt >= request.Dt1 && e.CreateAt <= request.Dt2 && (request.Khdb.IsNullOrEmpty() || e.Khdb == request.Khdb) && (request.Khmc.IsNullOrEmpty() || e.Khmc == request.Khmc))

            //    .Select<Goods,OrderGoods,OrderCost>((goods,ordergoods,ordercost)=>new {
            //        Name= Sql.Max(ordergoods.Name),
            //        Dhsl = Sql.As(Sql.Sum(ordergoods.Num), "Dhsl"),
            //        Dzsl = Sql.As(Sql.Sum(ordergoods.Dz), "Dzsl"),
            //        Dkje = Sql.As(Sql.Sum(ordergoods.Shje), "Dkje"),
            //        Dzje = Sql.As(Sql.Sum(ordergoods.Dz*ordergoods.Price), "Dzje"),
            //        Fhje = Sql.As(Sql.Sum((ordergoods.Num+ ordergoods.Dz)*ordergoods.Price), "Fhje"),
            //       // Fyje = Sql.As(Sql.Sum(ordergoods.Dz * ordergoods.Price)*(decimal)0.67, "Fyje"),
            //      //  Dzje = Sql.As(Sql.Sum(ordergoods.Dz*ordergoods.Price), "Dzje"),
            //    })
            //    //  .GroupBy(e => e.ID)
            //    ;
            var sql = @"Select Goods.id, Max(Name) Name
, Sum(isnull(ordergoods.Num, 0)) Dhsl
, Sum(isnull(ordergoods.Dz, 0)) Dzsl
, Sum(isnull(ordergoods.Shje, 0)) as Dkje
, Sum(isnull(ordergoods.Dz, 0) * isnull(ordergoods.Price, 0)) as Dzje
, Sum((isnull(ordergoods.Num, 0) + isnull(ordergoods.Dz, 0)) * isnull(ordergoods.Price, 0)) as Fhje
 From Goods left join OrderGoods On Goods.Id = OrderGoods.GoodsID and isnull(OrderGoods.Del, 0) = 0 left join[Order] on[order].ID = OrderGoods.OrderID
where
Goods.[Type] = 2
and[order].CreateAt >= @Dt1 and[order].CreateAt <=@Dt2 and  isnull( [order].Del, 0)= 0 and (isnull(@Khjl,'')='' or [Order].Khdb=@Khjl) and (isnull(@Khmc,'')='' or [Order].Khmc=@Khmc)
group by goods.ID";
            var results = Db.Select<MemberReportHpResponse>(sql, new List<SqlParameter>() {
new SqlParameter("Dt1",request.Dt1),
new SqlParameter("Dt2",request.Dt2),
new SqlParameter("Khjl",request.Khjl??""),
new SqlParameter("Khmc",request.Khmc??""),
            });
            results.Add(new MemberReportHpResponse() {Name="合计",
                Dhsl =results.Sum(e=>e.Dhsl),
                Dzsl = results.Sum(e=>e.Dzsl),
                Dkje = results.Sum(e=>e.Dkje),
                Dzje = results.Sum(e=>e.Dzje),
                Fhje = results.Sum(e=>e.Fhje),
                Fyje = results.Sum(e=>e.Fyje),
                Yj = results.Sum(e=>e.Yj),
              
            });
            return new { Results = results
               

            };
        }




        public MemberReportGdIndexResponse Get(MemberReportGd request)
        {
            var customers = Db.Select<Customer>(Db.From<Customer>().Select(e => new { e.Khmc, e.ID }).OrderBy(e => new { e.Khmc }));
            var users = Db.Select<UserAuth>(Db.From<UserAuth>().Select(e => new { e.DisplayName, e.Id }).OrderBy(e => new { e.DisplayName }));
            return new MemberReportGdIndexResponse() { Customers = customers, Users = users };
        }
        [Authenticate]
        public object Post(MemberReportGd request)
        {
            //var sql = Db.From<Goods>().LeftJoin<OrderGoods>((g, o) => g.ID == o.GoodsID && !o.Del)
            //    .LeftJoin<Order,OrderGoods >(( o,g) => g.OrderID == o.ID)
            //    .LeftJoin<Order,OrderCost >(( o,g) => g.OrderID == o.ID)

            //    .Where(e => e.Type==2)
            //    .And<Order>(e => e.CreateAt >= request.Dt1 && e.CreateAt <= request.Dt2 && (request.Khdb.IsNullOrEmpty() || e.Khdb == request.Khdb) && (request.Khmc.IsNullOrEmpty() || e.Khmc == request.Khmc))

            //    .Select<Goods,OrderGoods,OrderCost>((goods,ordergoods,ordercost)=>new {
            //        Name= Sql.Max(ordergoods.Name),
            //        Dhsl = Sql.As(Sql.Sum(ordergoods.Num), "Dhsl"),
            //        Dzsl = Sql.As(Sql.Sum(ordergoods.Dz), "Dzsl"),
            //        Dkje = Sql.As(Sql.Sum(ordergoods.Shje), "Dkje"),
            //        Dzje = Sql.As(Sql.Sum(ordergoods.Dz*ordergoods.Price), "Dzje"),
            //        Fhje = Sql.As(Sql.Sum((ordergoods.Num+ ordergoods.Dz)*ordergoods.Price), "Fhje"),
            //       // Fyje = Sql.As(Sql.Sum(ordergoods.Dz * ordergoods.Price)*(decimal)0.67, "Fyje"),
            //      //  Dzje = Sql.As(Sql.Sum(ordergoods.Dz*ordergoods.Price), "Dzje"),
            //    })
            //    //  .GroupBy(e => e.ID)
            //    ;
            var sql = @"Select Goods.id, Max(Name) Name
, Max(Category) Category
,Sum(isnull( ordergoods.Num,0)) Dhsl
,Sum(isnull( ordergoods.Dz,0)) Dzsl
,max(Price) Price
,Sum(isnull(ordergoods.Shje,0))as Dkje
,Sum(isnull(ordergoods.Dz,0)*isnull(ordergoods.Price,0))as Dzje
,Sum(case when [type]=1 then (isnull(ordergoods.Num,0)+ isnull(ordergoods.Dz,0))*isnull(ordergoods.Price,0) else 0 end) Fhje
,Sum(case when [type]=3 then (isnull(ordergoods.Num,0)+ isnull(ordergoods.Dz,0))*isnull(ordergoods.Price,0) else 0 end) Szhpfhje
 From Goods left join OrderGoods On Goods.Id = OrderGoods.GoodsID and isnull(OrderGoods.Del, 0) = 0 left join[Order] on[order].ID = OrderGoods.OrderID
where
Goods.[Type] in (1,3)
and[order].CreateAt >= @Dt1 and[order].CreateAt <=@Dt2 and  isnull( [order].Del, 0)= 0 and (isnull(@Khjl,'')='' or [Order].Khdb=@Khjl) and (isnull(@Khmc,'')='' or [Order].Khmc=@Khmc)
group by goods.ID";
            var results = Db.Select<MemberReportGdResponse>(sql, new List<SqlParameter>() {
new SqlParameter("Dt1",request.Dt1),
new SqlParameter("Dt2",request.Dt2),
new SqlParameter("Khjl",request.Khjl??""),
new SqlParameter("Khmc",request.Khmc??""),
            });
            results.Add(new MemberReportGdResponse()
            {
                Name = "合计",
                Dhsl = results.Sum(e => e.Dhsl),
                Dzsl = results.Sum(e => e.Dzsl),
                Dkje = results.Sum(e => e.Dkje),
                Dzje = results.Sum(e => e.Dzje),
                Fhje = results.Sum(e => e.Fhje),             
                Szhpfhje = results.Sum(e => e.Szhpfhje),
                Yj = results.Sum(e => e.Yj),

            });
            return new
            {
                Results = results


            };
        }
    }
}
