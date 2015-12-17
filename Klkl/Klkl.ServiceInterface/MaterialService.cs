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
    public class MaterialService:Service
    {
        public object Post(UpdateMaterial request)
        {
            var data = request.Material;
            if (data.ID==0)
            {
                data.ID = (int) Db.Insert(data, true);
            }
            else
            {
                Db.Update(data);
            }
            return data.ID;
        }

        public object Post(DelMaterial request)
        {
            return Db.DeleteById<Material>(request.ID);
        }

        public object Post(GetMaterials request)
        {
            GetMaterialsResponse response=new GetMaterialsResponse();
            response.Materials= Db.Select<Material>();
            response.MaterialTypes= Db.Select<MaterialType>();
            foreach (var material in response.Materials)
            {
                var type = response.MaterialTypes.FirstOrDefault(e => e.ID == material.TypeID);
                material.TypeName = type == null ? "" : type.Name;
                material.Unit = type == null ? "" : type.Unit;

            }
            return response;
        }

        public object Post(UpdateMaterialType request)
        {
            var data = request.MaterialType;
            if (data.ID == 0)
            {
                data.ID = (int)Db.Insert(data, true);
            }
            else
            {
                Db.Update(data);
            }
            return data.ID;
        }

        public object Post(DelMaterialType request)
        {
            return Db.DeleteById<MaterialType>(request.ID);
        }

        public object Post(GetMaterialTypes request)
        {
            return Db.Select<MaterialType>();
        }
    }
}
