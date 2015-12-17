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
 //   [Authenticate]
    public class CostService:Service
    {
        public object Post(UpdateCost request)
        {
            if (request.Cost.ID>0)
            {
                Db.Update(request.Cost);
            }
            else
            {
                request.Cost.ID = (int) Db.Insert(request.Cost, true);
            }
            return request.Cost.ID;
        }

        public object Post(DelCost request)
        {
            Db.DeleteById<Cost>(request.ID);
            return true;
        }

        public object Post(GetCost request)
        {
            return Db.SingleById<Cost>(request.ID);
        }

        public object Post(GetCosts request)
        {
            return Db.Select<Cost>();
        }
    }
}
