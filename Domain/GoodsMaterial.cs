using ServiceStack.DataAnnotations;

namespace Domain
{
    public class GoodsMaterial
    {
        [AutoIncrement]
        public int ID { get; set; }

        public int GoodsID { get; set; }

        public int MaterialID { get; set; }

        public decimal Num { get; set; }
        [Ignore]
        public string MaterialTypeName { get; set; }
        [Ignore]
        public int MaterialTypeID { get; set; }
        [Ignore]
        public string MaterialName { get; set; }
        [Ignore]
        public string Unit { get; set; }
    }
}