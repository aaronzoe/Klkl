﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
   // [Authenticate]
    public class OrderService:Service
    {
        public IAutoQuery AutoQuery { get; set; }
        public object Post(OrderList request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            var result = AutoQuery.Execute(request, query);
         //   return result;
            return new {total = result.Total, result = result.Results};

        }
        [Authenticate]
        public object Get(OrderList request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            var result = AutoQuery.Execute(request, query);
            return new { total = result.Total, result = result.Results };

        }
    
        public object Get(OrderIndex request)
        {
           // Response.Redirect("/#/page/login");
            return new object();
      
        }
    }
}
