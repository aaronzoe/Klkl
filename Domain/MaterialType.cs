using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace Domain
{
    public class MaterialType
    {
        [AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }
        [Ignore]
        public IList<Material> Materials { get; set; } 

 
    }
}