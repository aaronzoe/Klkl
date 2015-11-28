using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/account/{id}","GET")]
    public class AccountInfo
    {
        public int Id { get; set; }
    }
   
}
