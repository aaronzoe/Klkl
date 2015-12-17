using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/customer/update","POST")]
    public class CustomerUpdate
    {
        public Customer Customer { get; set; }
    }
    [Route("/customer/list","GET")]
    public class GetCustomerList:QueryBase<Customer>
    {
        
    }

    [Route("/customer", "POST")]
    public class GetCustomer
    {
        public int ID { get; set; }
    }
    [Route("/customer/del", "POST")]
    public class DelCustomer
    {
        public int ID { get; set; }
    }
}
