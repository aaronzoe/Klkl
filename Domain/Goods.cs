using ServiceStack.DataAnnotations;

namespace Domain
{
    public class Goods
    {
        [AutoIncrement]
        public int ID { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public string Size { get; set; }

        public decimal Price1 { get; set; }

        public decimal Price2 { get; set; }
        [Ignore]
        public string NewName
        {
            get { return Category + "-" + Name; }
        }

        public decimal Weight { get; set; }

        public decimal Volume { get; set; }
    }
}