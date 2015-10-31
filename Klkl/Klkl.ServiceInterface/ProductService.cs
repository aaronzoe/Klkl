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
    public class ProductService:Service
    {
        public object Get(Product request)
        {
            ProductResponse response = new ProductResponse();
            response.MaterialTypes = Db.Select<MaterialType>();
            response.Categories = Db.Select<Category>();
            response.Goods = request.ID > 0 ? Db.SingleById<Goods>(request.ID) : new Goods();
            return response;
        }

        public object Get(Products request)
        {
            ProductsResponse response=new ProductsResponse();
            response.Goodses = Db.Select<Goods>();
            return response;
        }
    }
}
