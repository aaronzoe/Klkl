using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/categories","GET")]
    public class Categories
    {
        
    }

    [Route("/category/update", "POST")]
    public class Update
    {
        public Category Category { get; set; }
    }
    [Route("/category/del", "POST")]
    public class Del
    {
        public int Id { get; set; }
    }
}
