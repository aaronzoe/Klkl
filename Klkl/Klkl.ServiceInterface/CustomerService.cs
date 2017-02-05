using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
    [Authenticate]
    public class CustomerService:Service
    {
        public IAutoQueryDb AutoQuery { get; set; }
        public object Post(CustomerUpdate request)
        {
            if (request.Customer.ID>0)
            {
                Db.Update(request.Customer);
            }
            else
            {
                request.Customer.ID= (int)Db.Insert(request.Customer, true);
            }
            return request.Customer.ID;
        }

        public object Get(GetCustomerList request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            var result = AutoQuery.Execute(request, query);
            return new { total = result.Total, result = result.Results };
        }

        public IList<Customer> GetCustomers()
        {
            return Db.Select<Customer>();
        } 

        public object Post(GetCustomer request)
        {
            return Db.SingleById<Customer>(request.ID);
        }

        public object Post(DelCustomer request)
        {
            return Db.DeleteById<Customer>(request.ID);
        }

        
    }
}
