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
   public class CategoryService:Service
    {
       public object Get(Categories request)
       {
           var cacheKey = UrnId.Create<Categories>("all");
           return Request.ToOptimizedResultUsingCache(Cache, cacheKey, () => { return Db.Select<Category>(); });
       }

       public object Post(Update request)
       {
           if (request.Category.ID==0)
           {
               request.Category.ID = (int) Db.Insert(request.Category, true);
           }
           else
           {
                Db.Update(request.Category);
            }
           var cacheKey = UrnId.Create<Categories>("all");
           Request.RemoveFromCache(Cache,cacheKey);
            return request.Category.ID;
       }

       public object Post(Del request)
       {
           Db.DeleteById<Category>(request.Id);
            var cacheKey = UrnId.Create<Categories>("all");
            Request.RemoveFromCache(Cache, cacheKey);
            return new object();
        }
    }
}
