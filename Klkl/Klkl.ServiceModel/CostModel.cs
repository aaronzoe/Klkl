using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/cost/update","POST")]
    public class UpdateCost
    {
        public Cost Cost { get; set; }
    }
    [Route("/cost/del","POST")]
    public class DelCost
    {
        public int ID { get; set; }
    }
    [Route("/cost/get")]
    public class GetCost
    {
        public int ID { get; set; }
    }
    [Route("/cost/getlist")]
    public class GetCosts
    {
     
    }
}
