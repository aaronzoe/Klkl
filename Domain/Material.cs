using System.Drawing;
using ServiceStack.DataAnnotations;

namespace Domain
{
    public class Material
    {
        [AutoIncrement]
        public int ID { get; set; }

        public int TypeID { get; set; }

        public decimal Amount { get; set; }

        public string Name { get; set; }

        public decimal Limit { get; set; }
        [Ignore]
        public Color Color {
            get
            {
                if (Limit==0)
                {
                    return Color.White;
                }
                if (Amount <= Limit)
                {
                    return ColorTranslator.FromHtml("#DB4D6D");
                }
                if ((Amount - Limit)/ Limit < (decimal)0.1)
                {
                    return Color.DarkOrange;
                    
                }
                return Color.White;
            }
        }
        [Ignore]
        public string Unit { get; set; }
        [Ignore]
        public string TypeName { get; set; }
    }
}