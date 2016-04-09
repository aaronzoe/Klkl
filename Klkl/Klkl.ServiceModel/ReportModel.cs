using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/report/product")]
    public class ProductReport
    {
        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }
    }

    [Route("/report/customer")]
    public class CustomerReport
    {
        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }
    }
}
