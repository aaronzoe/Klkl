using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Domain
{
   public class Cost
    {
        [AutoIncrement]
        public int ID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
    }
}
