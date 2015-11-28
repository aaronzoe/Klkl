using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace Domain
{
    public class Order
    {
        public Order()
        {
            CreateAt = DateTime.Now;
            Yjfhsj=DateTime.Now;
        }
        [AutoIncrement]
        public int ID { get; set; }

        public string OrderID { get; set; }

        public string Khqd { get; set; }
        public string Khmc { get; set; }
        public string Lxr { get; set; }
        public string Lxdh { get; set; }
        public DateTime Yjfhsj { get; set; }
        public string Shdz { get; set; }
        public string UserID { get; set; }
        public string Zt { get; set; }
        public decimal Shje { get; set; }
        public string Remark { get; set; }
        public bool NeedSp { get; set; }
        [Ignore]
        public IList<OrderGoods> OrderGoodses { get; set; }
        [Ignore]
        public IList<OrderCost> OrderCosts { get; set; }
        public string AreaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateAt { get; set; }

       
        public bool Del { get; set; }


        [Ignore]
        public decimal Dk { get; set; }
        [Ignore]
        public decimal Yf { get; set; }
        [Ignore]
        public decimal Zje { get; set; }
        [Ignore]
        public decimal Fy { get; set; }
        [Ignore]
        public Int64 Xh { get; set; }

    }
    public class OrderGoods
    {
              [AutoIncrement]
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int GoodsID { get; set; }


        public string Taste { get; set; }
        public int Num { get; set; }
        public decimal Price { get; set; }
         public decimal Amount { get; set; }

        public string Remark { get; set; }
         /// <summary>
         /// 运费
         /// </summary>
         public decimal Fare { get; set; }
         /// <summary>
         /// 实付金额
         /// </summary>
         public decimal Shje { get; set; }

     /// <summary>
     /// 重量
     /// </summary>
     public decimal Weight { get; set; }
         /// <summary>
         /// 搭赠
         /// </summary>
     public int Dz { get; set; }
        [Ignore]

         public Int64 Xh { get; set; }
        [Ignore]
        public string Category { get; set; }
        [Ignore]
        public string Name { get; set; }
        [Ignore]
        public string Code { get; set; }
        [Ignore]
        public string Size { get; set; }
        [Ignore]
        public string NewName { get; set; }
    }
}