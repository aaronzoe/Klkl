using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klkl.ServiceModel;
using ServiceStack;

namespace Klkl.ServiceInterface
{
    [Authenticate]
   public class ReportService:Service
    {
        [RequiredRole("Admin","总经理")]
        public object Post(ProductReport request)
       {
           var products = ResolveService<ProductService>().Get(new Products()).Goodses;
           var os = ResolveService<OrderService>();
           var orders = os.GetAllOrders();
           var ordergoodses = os.GetAllOrderGoodses().Where(e=>orders.Any(o=>o.ID==e.OrderID));

           foreach (var product in products)
           {
               var gs = ordergoodses.Where(e => e.GoodsID == product.ID);
               product.SellNum = gs.Sum(e => e.Num);
               product.SellAmount = gs.Sum(e => e.Amount);
            }
           return products;
       }

        [RequiredRole("Admin", "总经理")]
        public object Post(CustomerReport request)
        {
            var customers = ResolveService<CustomerService>().GetCustomers();
            var os = ResolveService<OrderService>();
            var orders = os.GetAllOrders();
            var ordergoodses = os.GetAllOrderGoodses().Where(e => orders.Any(o => o.ID == e.OrderID));

            foreach (var customer in customers)
            {
                var corders = orders.Where(o => o.Khmc == customer.Khmc);
                customer.OrderNum = corders.Count();
                customer.OrderAmount = ordergoodses.Where(g => corders.Any(o => o.ID == g.OrderID)).Sum(g => g.Amount);
            }
            return customers;
        }
    }
}
