using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace Domain
{
    public class Goods
    {
        public Goods ()
        {
            Size = "";

        }
        [AutoIncrement]
        public int ID { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public string Size { get; set; }

        public decimal Price1 { get; set; }

        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }

        [Ignore]
        public string NewName
        {
            get { return Category + "-" + Name; }
        }

        public decimal Weight { get; set; }

        public decimal Volume { get; set; }
        [StringLength(1000)]
        public IList<GoodsMaterial> Materials { get; set; }
        [Ignore]
        public int SellNum { get; set; }

        [Ignore]
        public decimal SellAmount { get; set; }
        /// <summary>
        /// 产品分类 1:果冻 2:含片
        /// </summary>
        public int Type { get; set; }
        ///// <summary>
        ///// 返利率
        ///// </summary>
        //public decimal Rate { get; set; }
    }
}